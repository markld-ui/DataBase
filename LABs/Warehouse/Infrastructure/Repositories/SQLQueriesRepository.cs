using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Interfaces;
using Npgsql;

namespace Infrastructure.Repositories
{
    /// <summary>
    /// Реализация репозитория для выполнения сложных SQL-запросов к базе данных PostgreSQL.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Класс реализует интерфейс <see cref="ISQLQueriesRepository"/> и предоставляет методы
    /// для выполнения сложных SQL-запросов, включая подзапросы, агрегатные функции и CRUD-операции.
    /// </para>
    /// <para>
    /// Все методы используют параметризованные запросы для защиты от SQL-инъекций и являются
    /// потокобезопасными за счёт использования отдельных подключений через <see cref="DatabaseConnection"/>.
    /// </para>
    /// <para>
    /// Для корректной работы требуется настроенное подключение к базе данных PostgreSQL.
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    /// var dbConnection = new DatabaseConnection("Host=localhost;Username=user;Password=pass;Database=products");
    /// var repository = new SQLQueriesRepository(dbConnection);
    /// var records = repository.GetAllRecords();
    /// foreach (DataRow row in records.Rows)
    /// {
    ///     Console.WriteLine($"ID: {row["productAcc_id"]}, Employee: {row["employee_name"]}");
    /// }
    /// </code>
    /// </example>
    public class SQLQueriesRepository : ISQLQueriesRepository
    {
        private readonly DatabaseConnection _dbConnection;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="SQLQueriesRepository"/>.
        /// </summary>
        /// <param name="dbConnection">Объект подключения к базе данных типа <see cref="DatabaseConnection"/>.</param>
        /// <exception cref="System.ArgumentNullException">
        /// Выбрасывается, если параметр <paramref name="dbConnection"/> равен null.
        /// </exception>
        public SQLQueriesRepository(DatabaseConnection dbConnection)
        {
            _dbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
        }

        /// <summary>
        /// Получает все записи учета продуктов.
        /// </summary>
        /// <returns>
        /// Объект <see cref="DataTable"/> с полным набором записей учета продуктов,
        /// включая идентификатор записи, дату учета, количество, имя сотрудника и название зоны хранения.
        /// </returns>
        /// <remarks>
        /// Метод выполняет SQL-запрос с использованием INNER JOIN для получения связанных данных
        /// из таблиц employee и storage_zone.
        /// </remarks>
        public DataTable GetAllRecords()
        {
            using (var conn = _dbConnection.GetConnection())
            {
                conn.Open();
                string query = @"
                    SELECT pa.productAcc_id, pa.accounting_date, pa.quantity, 
                           e.full_name AS employee_name, 
                           sz.zone_name AS storage_zone
                    FROM product_accounting pa
                    INNER JOIN employee e ON pa.employee_id = e.employee_id
                    INNER JOIN storage_zone sz ON pa.storage_id = sz.storage_id";

                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    using (var adapter = new NpgsqlDataAdapter(cmd))
                    {
                        DataTable table = new DataTable();
                        adapter.Fill(table);
                        return table;
                    }
                }
            }
        }

        /// <summary>
        /// Получает записи учета продуктов по идентификатору сотрудника.
        /// </summary>
        /// <param name="employeeId">Идентификатор сотрудника (положительное число).</param>
        /// <returns>
        /// Объект <see cref="DataTable"/> с записями, связанными с указанным сотрудником,
        /// включая идентификатор записи, дату учета, количество, имя сотрудника и название зоны хранения.
        /// </returns>
        /// <exception cref="System.ArgumentException">
        /// Выбрасывается, если <paramref name="employeeId"/> меньше или равен 0.
        /// </exception>
        /// <remarks>
        /// Метод проверяет существование сотрудника перед выполнением запроса.
        /// </remarks>
        public DataTable GetRecordsByEmployee(int employeeId)
        {
            if (employeeId <= 0)
                throw new ArgumentException("Идентификатор сотрудника должен быть больше нуля.", nameof(employeeId));

            if (!EmployeeExists(employeeId))
                throw new ArgumentException($"Сотрудник с идентификатором {employeeId} не существует.", nameof(employeeId));

            using (var conn = _dbConnection.GetConnection())
            {
                conn.Open();
                string query = @"
                    SELECT pa.productAcc_id, pa.accounting_date, pa.quantity, 
                           e.full_name AS employee_name, 
                           sz.zone_name AS storage_zone
                    FROM product_accounting pa
                    INNER JOIN employee e ON pa.employee_id = e.employee_id
                    INNER JOIN storage_zone sz ON pa.storage_id = sz.storage_id
                    WHERE pa.employee_id = @employeeId";

                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("employeeId", employeeId);
                    using (var adapter = new NpgsqlDataAdapter(cmd))
                    {
                        DataTable table = new DataTable();
                        adapter.Fill(table);
                        return table;
                    }
                }
            }
        }

        /// <summary>
        /// Получает агрегированные данные учета продуктов.
        /// </summary>
        /// <param name="minRecordCount">Минимальное количество записей для включения в результат.</param>
        /// <param name="startDate">Начальная дата для фильтрации записей.</param>
        /// <returns>
        /// Объект <see cref="DataTable"/> с агрегированными данными, содержащий
        /// название зоны хранения, количество записей, сумму и среднее значение количества.
        /// </returns>
        /// <exception cref="System.ArgumentException">
        /// Выбрасывается, если <paramref name="minRecordCount"/> меньше 0
        /// или <paramref name="startDate"/> находится в будущем относительно текущей даты.
        /// </exception>
        /// <remarks>
        /// Метод использует GROUP BY и HAVING для фильтрации данных.
        /// </remarks>
        public DataTable GetAggregateRecords(int minRecordCount, DateTime startDate)
        {
            if (minRecordCount < 0)
                throw new ArgumentException("Минимальное количество записей не может быть отрицательным.", nameof(minRecordCount));

            if (startDate > DateTime.UtcNow)
                throw new ArgumentException("Начальная дата не может быть в будущем.", nameof(startDate));

            using (var conn = _dbConnection.GetConnection())
            {
                conn.Open();
                string query = @"
                    SELECT sz.zone_name AS storage_zone, 
                           COUNT(pa.productAcc_id) AS record_count, 
                           SUM(pa.quantity) AS total_quantity,
                           AVG(pa.quantity) AS avg_quantity
                    FROM product_accounting pa
                    INNER JOIN storage_zone sz ON pa.storage_id = sz.storage_id
                    WHERE pa.accounting_date >= @startDate
                    GROUP BY sz.zone_name
                    HAVING COUNT(pa.productAcc_id) >= @minRecordCount
                    ORDER BY total_quantity DESC";

                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("minRecordCount", minRecordCount);
                    cmd.Parameters.AddWithValue("startDate", startDate);

                    using (var adapter = new NpgsqlDataAdapter(cmd))
                    {
                        DataTable table = new DataTable();
                        adapter.Fill(table);
                        return table;
                    }
                }
            }
        }

        /// <summary>
        /// Получает упрощенные записи учета продуктов (без связанных данных).
        /// </summary>
        /// <returns>
        /// Объект <see cref="DataTable"/> с полями productAcc_id, accounting_date, quantity,
        /// employee_id, supply_id, storage_id.
        /// </returns>
        /// <remarks>
        /// Метод возвращает базовые данные из таблицы product_accounting без JOIN-операций.
        /// </remarks>
        public DataTable GetSimpleProductAccountingRecords()
        {
            using (var conn = _dbConnection.GetConnection())
            {
                conn.Open();
                string query = @"
                    SELECT productAcc_id, accounting_date, quantity, employee_id, supply_id, storage_id
                    FROM product_accounting";

                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    using (var adapter = new NpgsqlDataAdapter(cmd))
                    {
                        DataTable table = new DataTable();
                        adapter.Fill(table);
                        return table;
                    }
                }
            }
        }

        /// <summary>
        /// Выполняет коррелированный подзапрос для получения связанных данных поставки.
        /// </summary>
        /// <param name="supplyId">Идентификатор поставки (положительное число).</param>
        /// <returns>
        /// Объект <see cref="DataTable"/> с данными учета продуктов, датой поставки и именем сотрудника.
        /// </returns>
        /// <exception cref="System.ArgumentException">
        /// Выбрасывается, если <paramref name="supplyId"/> меньше или равен 0.
        /// </exception>
        /// <remarks>
        /// Метод использует коррелированные подзапросы для получения данных из таблиц supply и employee.
        /// </remarks>
        public DataTable GetCorrelatedSubquery(int supplyId)
        {
            if (supplyId <= 0)
                throw new ArgumentException("Идентификатор поставки должен быть больше нуля.", nameof(supplyId));

            if (!SupplyExists(supplyId))
                throw new ArgumentException($"Поставка с идентификатором {supplyId} не существует.", nameof(supplyId));

            using (var conn = _dbConnection.GetConnection())
            {
                conn.Open();
                string query = @"
                    SELECT pa.productAcc_id, pa.accounting_date, pa.quantity,
                           (SELECT s.supply_date FROM supply s WHERE s.supply_id = pa.supply_id) AS supply_date,
                           (SELECT e.full_name FROM employee e WHERE e.employee_id = pa.employee_id) AS employee_name
                    FROM product_accounting pa
                    WHERE pa.supply_id = @supplyId";

                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("supplyId", supplyId);
                    using (var adapter = new NpgsqlDataAdapter(cmd))
                    {
                        DataTable table = new DataTable();
                        adapter.Fill(table);
                        return table;
                    }
                }
            }
        }

        /// <summary>
        /// Выполняет некоррелированный подзапрос для получения связанных данных поставки.
        /// </summary>
        /// <param name="supplyId">Идентификатор поставки (положительное число).</param>
        /// <returns>
        /// Объект <see cref="DataTable"/> с данными учета продуктов, где количество превышает
        /// среднее значение для указанной поставки, и именем сотрудника.
        /// </returns>
        /// <exception cref="System.ArgumentException">
        /// Выбрасывается, если <paramref name="supplyId"/> меньше или равен 0.
        /// </exception>
        /// <remarks>
        /// Метод использует некоррелированный подзапрос для вычисления среднего количества.
        /// </remarks>
        public DataTable GetNonCorrelatedSubquery(int supplyId)
        {
            if (supplyId <= 0)
                throw new ArgumentException("Идентификатор поставки должен быть больше нуля.", nameof(supplyId));

            if (!SupplyExists(supplyId))
                throw new ArgumentException($"Поставка с идентификатором {supplyId} не существует.", nameof(supplyId));

            using (var conn = _dbConnection.GetConnection())
            {
                conn.Open();
                string query = @"
                    SELECT pa.productAcc_id, pa.accounting_date, pa.quantity,
                           e.full_name AS employee_name
                    FROM product_accounting pa
                    INNER JOIN employee e ON pa.employee_id = e.employee_id
                    WHERE pa.supply_id = @supplyId
                    AND pa.quantity > (SELECT AVG(quantity) FROM product_accounting WHERE supply_id = @supplyId)";

                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("supplyId", supplyId);
                    using (var adapter = new NpgsqlDataAdapter(cmd))
                    {
                        DataTable table = new DataTable();
                        adapter.Fill(table);
                        return table;
                    }
                }
            }
        }

        /// <summary>
        /// Проверяет существование сотрудника по идентификатору.
        /// </summary>
        /// <param name="employeeId">Идентификатор сотрудника (положительное число).</param>
        /// <returns>
        /// <c>true</c>, если сотрудник существует; <c>false</c> в противном случае.
        /// </returns>
        /// <exception cref="System.ArgumentException">
        /// Выбрасывается, если <paramref name="employeeId"/> меньше или равен 0.
        /// </exception>
        public bool EmployeeExists(int employeeId)
        {
            if (employeeId <= 0)
                throw new ArgumentException("Идентификатор сотрудника должен быть больше нуля.", nameof(employeeId));

            using (var conn = _dbConnection.GetConnection())
            {
                conn.Open();
                string query = "SELECT EXISTS (SELECT 1 FROM employee WHERE employee_id = @employeeId)";

                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("employeeId", employeeId);
                    return (bool)cmd.ExecuteScalar();
                }
            }
        }

        /// <summary>
        /// Проверяет существование поставки по идентификатору.
        /// </summary>
        /// <param name="supplyId">Идентификатор поставки (положительное число).</param>
        /// <returns>
        /// <c>true</c>, если поставка существует; <c>false</c> в противном случае.
        /// </returns>
        /// <exception cref="System.ArgumentException">
        /// Выбрасывается, если <paramref name="supplyId"/> меньше или равен 0.
        /// </exception>
        public bool SupplyExists(int supplyId)
        {
            if (supplyId <= 0)
                throw new ArgumentException("Идентификатор поставки должен быть больше нуля.", nameof(supplyId));

            using (var conn = _dbConnection.GetConnection())
            {
                conn.Open();
                string query = "SELECT EXISTS (SELECT 1 FROM supply WHERE supply_id = @supplyId)";

                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("supplyId", supplyId);
                    return (bool)cmd.ExecuteScalar();
                }
            }
        }

        /// <summary>
        /// Проверяет существование зоны хранения по идентификатору.
        /// </summary>
        /// <param name="storageId">Идентификатор зоны хранения (положительное число).</param>
        /// <returns>
        /// <c>true</c>, если зона хранения существует; <c>false</c> в противном случае.
        /// </returns>
        /// <exception cref="System.ArgumentException">
        /// Выбрасывается, если <paramref name="storageId"/> меньше или равен 0.
        /// </exception>
        public bool StorageZoneExists(int storageId)
        {
            if (storageId <= 0)
                throw new ArgumentException("Идентификатор зоны хранения должен быть больше нуля.", nameof(storageId));

            using (var conn = _dbConnection.GetConnection())
            {
                conn.Open();
                string query = "SELECT EXISTS (SELECT 1 FROM storage_zone WHERE storage_id = @storageId)";

                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("storageId", storageId);
                    return (bool)cmd.ExecuteScalar();
                }
            }
        }

        /// <summary>
        /// Проверяет существование записи учета по идентификатору.
        /// </summary>
        /// <param name="recordId">Идентификатор записи учета (положительное число).</param>
        /// <returns>
        /// <c>true</c>, если запись существует; <c>false</c> в противном случае.
        /// </returns>
        /// <exception cref="System.ArgumentException">
        /// Выбрасывается, если <paramref name="recordId"/> меньше или равен 0.
        /// </exception>
        public bool RecordExists(int recordId)
        {
            if (recordId <= 0)
                throw new ArgumentException("Идентификатор записи учета должен быть больше нуля.", nameof(recordId));

            using (var conn = _dbConnection.GetConnection())
            {
                conn.Open();
                string query = "SELECT EXISTS (SELECT 1 FROM product_accounting WHERE productAcc_id = @recordId)";

                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("recordId", recordId);
                    return (bool)cmd.ExecuteScalar();
                }
            }
        }

        /// <summary>
        /// Добавляет новую запись учета продукта.
        /// </summary>
        /// <param name="accountingDate">Дата учета.</param>
        /// <param name="quantity">Количество продуктов.</param>
        /// <param name="employeeId">Идентификатор сотрудника.</param>
        /// <param name="supplyId">Идентификатор поставки.</param>
        /// <param name="storageId">Идентификатор зоны хранения.</param>
        /// <exception cref="System.ArgumentException">
        /// Выбрасывается, если:
        /// - любой идентификатор (<paramref name="employeeId"/>, <paramref name="supplyId"/>, <paramref name="storageId"/>) меньше или равен 0;
        /// - <paramref name="quantity"/> меньше или равен 0;
        /// - <paramref name="accountingDate"/> находится в будущем.
        /// </exception>
        /// <exception cref="System.InvalidOperationException">
        /// Выбрасывается, если:
        /// - сотрудник с указанным <paramref name="employeeId"/> не существует;
        /// - поставка с указанным <paramref name="supplyId"/> не существует;
        /// - зона хранения с указанным <paramref name="storageId"/> не существует.
        /// </exception>
        /// <remarks>
        /// Метод выполняется в рамках транзакции для обеспечения атомарности.
        /// </remarks>
        public void InsertRecord(DateTime accountingDate, int quantity, int employeeId, int supplyId, int storageId)
        {
            if (employeeId <= 0)
                throw new ArgumentException("Идентификатор сотрудника должен быть больше нуля.", nameof(employeeId));

            if (supplyId <= 0)
                throw new ArgumentException("Идентификатор поставки должен быть больше нуля.", nameof(supplyId));

            if (storageId <= 0)
                throw new ArgumentException("Идентификатор зоны хранения должен быть больше нуля.", nameof(storageId));

            if (quantity <= 0)
                throw new ArgumentException("Количество продуктов должно быть больше нуля.", nameof(quantity));

            if (accountingDate > DateTime.UtcNow)
                throw new ArgumentException("Дата учета не может быть в будущем.", nameof(accountingDate));

            if (!EmployeeExists(employeeId))
                throw new InvalidOperationException($"Сотрудник с идентификатором {employeeId} не существует.");

            if (!SupplyExists(supplyId))
                throw new InvalidOperationException($"Поставка с идентификатором {supplyId} не существует.");

            if (!StorageZoneExists(storageId))
                throw new InvalidOperationException($"Зона хранения с идентификатором {storageId} не существует.");

            using (var conn = _dbConnection.GetConnection())
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        string query = @"
                            INSERT INTO product_accounting (accounting_date, quantity, employee_id, supply_id, storage_id)
                            VALUES (@accountingDate, @quantity, @employeeId, @supplyId, @storageId)";

                        using (var cmd = new NpgsqlCommand(query, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("accountingDate", accountingDate);
                            cmd.Parameters.AddWithValue("quantity", quantity);
                            cmd.Parameters.AddWithValue("employeeId", employeeId);
                            cmd.Parameters.AddWithValue("supplyId", supplyId);
                            cmd.Parameters.AddWithValue("storageId", storageId);
                            cmd.ExecuteNonQuery();
                        }

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new InvalidOperationException($"Не удалось добавить запись учета. Ошибка: {ex.Message}", ex);
                    }
                }
            }
        }

        /// <summary>
        /// Обновляет существующую запись учета продукта.
        /// </summary>
        /// <param name="id">Идентификатор записи (положительное число).</param>
        /// <param name="accountingDate">Новая дата учета (null, если не требуется обновление).</param>
        /// <param name="quantity">Новое количество (null, если не требуется обновление).</param>
        /// <param name="employeeId">Новый идентификатор сотрудника (null, если не требуется обновление).</param>
        /// <param name="supplyId">Новый идентификатор поставки (null, если не требуется обновление).</param>
        /// <param name="storageId">Новый идентификатор зоны хранения (null, если не требуется обновление).</param>
        /// <exception cref="System.ArgumentException">
        /// Выбрасывается, если:
        /// - <paramref name="id"/> меньше или равен 0;
        /// - <paramref name="quantity"/> меньше или равен 0 (если указано);
        /// - <paramref name="accountingDate"/> находится в будущем (если указано);
        /// - любой идентификатор (<paramref name="employeeId"/>, <paramref name="supplyId"/>, <paramref name="storageId"/>) меньше или равен 0 (если указано).
        /// </exception>
        /// <exception cref="System.InvalidOperationException">
        /// Выбрасывается, если:
        /// - запись с указанным <paramref name="id"/> не существует;
        /// - сотрудник с указанным <paramref name="employeeId"/> не существует (если указано);
        /// - поставка с указанным <paramref name="supplyId"/> не существует (если указано);
        /// - зона хранения с указанным <paramref name="storageId"/> не существует (если указано).
        /// </exception>
        /// <remarks>
        /// Метод выполняется в рамках транзакции для обеспечения атомарности.
        /// Если ни один параметр не указан, метод не выполняет обновление.
        /// </remarks>
        public void UpdateRecord(int id, DateTime? accountingDate, int? quantity, int? employeeId, int? supplyId, int? storageId)
        {
            if (id <= 0)
                throw new ArgumentException("Идентификатор записи должен быть больше нуля.", nameof(id));

            if (quantity.HasValue && quantity <= 0)
                throw new ArgumentException("Количество продуктов должно быть больше нуля.", nameof(quantity));

            if (accountingDate.HasValue && accountingDate > DateTime.UtcNow)
                throw new ArgumentException("Дата учета не может быть в будущем.", nameof(accountingDate));

            if (employeeId.HasValue && employeeId <= 0)
                throw new ArgumentException("Идентификатор сотрудника должен быть больше нуля.", nameof(employeeId));

            if (supplyId.HasValue && supplyId <= 0)
                throw new ArgumentException("Идентификатор поставки должен быть больше нуля.", nameof(supplyId));

            if (storageId.HasValue && storageId <= 0)
                throw new ArgumentException("Идентификатор зоны хранения должен быть больше нуля.", nameof(storageId));

            if (!RecordExists(id))
                throw new InvalidOperationException($"Запись с идентификатором {id} не существует.");

            if (employeeId.HasValue && !EmployeeExists(employeeId.Value))
                throw new InvalidOperationException($"Сотрудник с идентификатором {employeeId.Value} не существует.");

            if (supplyId.HasValue && !SupplyExists(supplyId.Value))
                throw new InvalidOperationException($"Поставка с идентификатором {supplyId.Value} не существует.");

            if (storageId.HasValue && !StorageZoneExists(storageId.Value))
                throw new InvalidOperationException($"Зона хранения с идентификатором {storageId.Value} не существует.");


            if (!accountingDate.HasValue && !quantity.HasValue && !employeeId.HasValue && !supplyId.HasValue && !storageId.HasValue)
                return; // Нет данных для обновления

            using (var conn = _dbConnection.GetConnection())
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        string query = "UPDATE product_accounting SET ";
                        var parameters = new List<NpgsqlParameter>();

                        if (accountingDate.HasValue)
                        {
                            query += "accounting_date = @accountingDate, ";
                            parameters.Add(new NpgsqlParameter("accountingDate", accountingDate.Value));
                        }

                        if (quantity.HasValue)
                        {
                            query += "quantity = @quantity, ";
                            parameters.Add(new NpgsqlParameter("quantity", quantity.Value));
                        }

                        if (employeeId.HasValue)
                        {
                            query += "employee_id = @employeeId, ";
                            parameters.Add(new NpgsqlParameter("employeeId", employeeId.Value));
                        }

                        if (supplyId.HasValue)
                        {
                            query += "supply_id = @supplyId, ";
                            parameters.Add(new NpgsqlParameter("supplyId", supplyId.Value));
                        }

                        if (storageId.HasValue)
                        {
                            query += "storage_id = @storageId, ";
                            parameters.Add(new NpgsqlParameter("storageId", storageId.Value));
                        }

                        query = query.TrimEnd(',', ' ') + " WHERE productAcc_id = @id";
                        parameters.Add(new NpgsqlParameter("id", id));

                        using (var cmd = new NpgsqlCommand(query, conn, transaction))
                        {
                            cmd.Parameters.AddRange(parameters.ToArray());
                            int rowsAffected = cmd.ExecuteNonQuery();
                            if (rowsAffected == 0)
                                throw new InvalidOperationException($"Запись с идентификатором {id} не найдена.");
                        }

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new InvalidOperationException($"Не удалось обновить запись. Ошибка: {ex.Message}", ex);
                    }
                }
            }
        }

        /// <summary>
        /// Удаляет запись учета продукта.
        /// </summary>
        /// <param name="id">Идентификатор записи (положительное число).</param>
        /// <exception cref="System.ArgumentException">
        /// Выбрасывается, если <paramref name="id"/> меньше или равен 0.
        /// </exception>
        /// <exception cref="System.InvalidOperationException">
        /// Выбрасывается, если запись с указанным <paramref name="id"/> не существует.
        /// </exception>
        /// <remarks>
        /// Метод выполняется в рамках транзакции для обеспечения атомарности.
        /// </remarks>
        public void DeleteRecord(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Идентификатор записи должен быть больше нуля.", nameof(id));

            if (!RecordExists(id))
                throw new InvalidOperationException($"Запись с идентификатором {id} не существует.");

            using (var conn = _dbConnection.GetConnection())
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        string query = "DELETE FROM product_accounting WHERE productAcc_id = @id";
                        using (var cmd = new NpgsqlCommand(query, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("id", id);
                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected == 0)
                                throw new InvalidOperationException($"Запись с идентификатором {id} не найдена.");
                        }

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new InvalidOperationException($"Не удалось удалить запись. Ошибка: {ex.Message}", ex);
                    }
                }
            }
        }
    }
}