using Domain.Interfaces;
using Domain.Models;
using Npgsql;

namespace Infrastructure.Repositories
{
    /// <summary>
    /// Реализация репозитория для работы с данными учета продуктов в базе данных PostgreSQL.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Класс реализует интерфейс <see cref="IProductAccountingRepository"/> и предоставляет методы для выполнения
    /// CRUD-операций и фильтрации данных учета продуктов с использованием PostgreSQL через библиотеку Npgsql.
    /// </para>
    /// <para>
    /// Все методы являются атомарными и потокобезопасными, так как используют отдельные подключения к базе данных,
    /// создаваемые через <see cref="DatabaseConnection"/>.
    /// </para>
    /// <para>
    /// Для корректной работы требуется настроенное подключение к базе данных PostgreSQL.
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    /// var dbConnection = new DatabaseConnection("Host=localhost;Username=user;Password=pass;Database=products");
    /// var repository = new ProductAccountingRepository(dbConnection);
    /// var productAccountings = repository.GetAll();
    /// foreach (var pa in productAccountings)
    /// {
    ///     Console.WriteLine($"ID: {pa.ProductAccId}, Quantity: {pa.Quantity}, Status: {pa.MovementStatus}");
    /// }
    /// </code>
    /// </example>
    public class ProductAccountingRepository : IProductAccountingRepository
    {
        private readonly DatabaseConnection _dbConnection;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ProductAccountingRepository"/>.
        /// </summary>
        /// <param name="dbConnection">Объект подключения к базе данных типа <see cref="DatabaseConnection"/>.</param>
        /// <exception cref="System.ArgumentNullException">
        /// Выбрасывается, если параметр <paramref name="dbConnection"/> равен null.
        /// </exception>
        public ProductAccountingRepository(DatabaseConnection dbConnection)
        {
            _dbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
        }

        /// <summary>
        /// Получает все записи учета продуктов из базы данных.
        /// </summary>
        /// <returns>
        /// Список <see cref="List{ProductAccounting}"/> всех записей учета продуктов.
        /// Если записи отсутствуют, возвращает пустой список.
        /// </returns>
        /// <remarks>
        /// Метод выполняет SQL-запрос для получения всех записей из таблицы product_accounting.
        /// </remarks>
        public List<ProductAccounting> GetAll()
        {
            var productAccountings = new List<ProductAccounting>();

            using (var conn = _dbConnection.GetConnection())
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("SELECT productAcc_id, supply_id, employee_id, storage_id, accounting_date, quantity, last_movement_date, movement_status FROM product_accounting", conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        productAccountings.Add(new ProductAccounting
                        {
                            ProductAccId = reader.GetInt32(0),
                            SupplyId = reader.GetInt32(1),
                            EmployeeId = reader.GetInt32(2),
                            StorageId = reader.GetInt32(3),
                            AccountingDate = reader.GetDateTime(4),
                            Quantity = reader.GetInt32(5),
                            LastMovementDate = reader.IsDBNull(6) ? null : reader.GetDateTime(6),
                            MovementStatus = reader.IsDBNull(7) ? "В наличии" : reader.GetString(7)
                        });
                    }
                }
            }

            return productAccountings;
        }

        /// <summary>
        /// Получает запись учета продукта по указанному идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор записи учета продукта (productAcc_id).</param>
        /// <returns>
        /// Объект <see cref="ProductAccounting"/> с данными записи или null, если запись не найдена.
        /// </returns>
        /// <exception cref="System.ArgumentException">
        /// Выбрасывается, если параметр <paramref name="id"/> меньше или равен 0.
        /// </exception>
        public ProductAccounting GetById(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Идентификатор записи учета продукта должен быть больше нуля.", nameof(id));

            using (var conn = _dbConnection.GetConnection())
            {
                conn.Open();

                using (var cmd = new NpgsqlCommand("SELECT productAcc_id, supply_id, employee_id, storage_id, accounting_date, quantity, last_movement_date, movement_status FROM product_accounting WHERE productAcc_id = @id", conn))
                {
                    cmd.Parameters.AddWithValue("id", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new ProductAccounting
                            {
                                ProductAccId = reader.GetInt32(0),
                                SupplyId = reader.GetInt32(1),
                                EmployeeId = reader.GetInt32(2),
                                StorageId = reader.GetInt32(3),
                                AccountingDate = reader.GetDateTime(4),
                                Quantity = reader.GetInt32(5),
                                LastMovementDate = reader.IsDBNull(6) ? null : reader.GetDateTime(6),
                                MovementStatus = reader.IsDBNull(7) ? "В наличии" : reader.GetString(7)
                            };
                        }

                        return null;
                    }
                }
            }
        }

        /// <summary>
        /// Добавляет новую запись учета продукта в базу данных.
        /// </summary>
        /// <param name="productAccounting">Объект записи учета продукта типа <see cref="ProductAccounting"/>.</param>
        /// <exception cref="System.ArgumentNullException">
        /// Выбрасывается, если параметр <paramref name="productAccounting"/> равен null.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// Выбрасывается, если обязательные поля <see cref="ProductAccounting.SupplyId"/>, 
        /// <see cref="ProductAccounting.EmployeeId"/>, <see cref="ProductAccounting.StorageId"/> или 
        /// <see cref="ProductAccounting.Quantity"/> меньше или равны нулю, либо если 
        /// <see cref="ProductAccounting.AccountingDate"/> имеет некорректное значение.
        /// </exception>
        /// <exception cref="System.InvalidOperationException">
        /// Выбрасывается, если запись с таким <see cref="ProductAccounting.ProductAccId"/> уже существует.
        /// </exception>
        /// <remarks>
        /// Метод устанавливает свойство <see cref="ProductAccounting.ProductAccId"/> на основе возвращаемого значения SQL-запроса.
        /// </remarks>
        public void Add(ProductAccounting productAccounting)
        {
            if (productAccounting == null)
                throw new ArgumentNullException(nameof(productAccounting));

            if (productAccounting.SupplyId <= 0)
                throw new ArgumentException("Идентификатор поставки должен быть больше нуля.", nameof(productAccounting.SupplyId));

            if (productAccounting.EmployeeId <= 0)
                throw new ArgumentException("Идентификатор сотрудника должен быть больше нуля.", nameof(productAccounting.EmployeeId));

            if (productAccounting.StorageId <= 0)
                throw new ArgumentException("Идентификатор склада должен быть больше нуля.", nameof(productAccounting.StorageId));

            if (productAccounting.Quantity <= 0)
                throw new ArgumentException("Количество продукта должно быть больше нуля.", nameof(productAccounting.Quantity));

            if (productAccounting.AccountingDate == default)
                throw new ArgumentException("Дата учета должна быть указана.", nameof(productAccounting.AccountingDate));

            using (var conn = _dbConnection.GetConnection())
            {
                conn.Open();

                using (var cmd = new NpgsqlCommand(
                    "INSERT INTO product_accounting (supply_id, employee_id, storage_id, accounting_date, quantity, last_movement_date, movement_status) VALUES (@supply_id, @employee_id, @storage_id, @accounting_date, @quantity, @last_movement_date, @movement_status) RETURNING productAcc_id", conn))
                {
                    cmd.Parameters.AddWithValue("supply_id", productAccounting.SupplyId);
                    cmd.Parameters.AddWithValue("employee_id", productAccounting.EmployeeId);
                    cmd.Parameters.AddWithValue("storage_id", productAccounting.StorageId);
                    cmd.Parameters.AddWithValue("accounting_date", productAccounting.AccountingDate);
                    cmd.Parameters.AddWithValue("quantity", productAccounting.Quantity);
                    cmd.Parameters.AddWithValue("last_movement_date", (object)productAccounting.LastMovementDate ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("movement_status", productAccounting.MovementStatus ?? "В наличии");
                    productAccounting.ProductAccId = (int)cmd.ExecuteScalar();
                }
            }
        }

        /// <summary>
        /// Обновляет существующую запись учета продукта в базе данных.
        /// </summary>
        /// <param name="productAccounting">Объект записи учета продукта типа <see cref="ProductAccounting"/> с обновлёнными данными.</param>
        /// <exception cref="System.ArgumentNullException">
        /// Выбрасывается, если параметр <paramref name="productAccounting"/> равен null.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// Выбрасывается, если <see cref="ProductAccounting.ProductAccId"/>, <see cref="ProductAccounting.SupplyId"/>, 
        /// <see cref="ProductAccounting.EmployeeId"/>, <see cref="ProductAccounting.StorageId"/> или 
        /// <see cref="ProductAccounting.Quantity"/> меньше или равны нулю, либо если 
        /// <see cref="ProductAccounting.AccountingDate"/> имеет некорректное значение.
        /// </exception>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException">
        /// Выбрасывается, если запись с указанным <see cref="ProductAccounting.ProductAccId"/> не найдена.
        /// </exception>
        public void Update(ProductAccounting productAccounting)
        {
            if (productAccounting == null)
                throw new ArgumentNullException(nameof(productAccounting));
            if (productAccounting.ProductAccId <= 0)
                throw new ArgumentException("Идентификатор записи учета продукта должен быть больше нуля.", nameof(productAccounting.ProductAccId));

            if (productAccounting.SupplyId <= 0)
                throw new ArgumentException("Идентификатор поставки должен быть больше нуля.", nameof(productAccounting.SupplyId));

            if (productAccounting.EmployeeId <= 0)
                throw new ArgumentException("Идентификатор сотрудника должен быть больше нуля.", nameof(productAccounting.EmployeeId));

            if (productAccounting.StorageId <= 0)
                throw new ArgumentException("Идентификатор склада должен быть больше нуля.", nameof(productAccounting.StorageId));

            if (productAccounting.Quantity <= 0)
                throw new ArgumentException("Количество продукта должно быть больше нуля.", nameof(productAccounting.Quantity));

            if (productAccounting.AccountingDate == default)
                throw new ArgumentException("Дата учета должна быть указана.", nameof(productAccounting.AccountingDate));

            using (var conn = _dbConnection.GetConnection())
            {
                conn.Open();

                using (var cmd = new NpgsqlCommand(
                    "UPDATE product_accounting SET supply_id = @supply_id, employee_id = @employee_id, storage_id = @storage_id, accounting_date = @accounting_date, quantity = @quantity, last_movement_date = @last_movement_date, movement_status = @movement_status WHERE productAcc_id = @id", conn))
                {
                    cmd.Parameters.AddWithValue("id", productAccounting.ProductAccId);
                    cmd.Parameters.AddWithValue("supply_id", productAccounting.SupplyId);
                    cmd.Parameters.AddWithValue("employee_id", productAccounting.EmployeeId);
                    cmd.Parameters.AddWithValue("storage_id", productAccounting.StorageId);
                    cmd.Parameters.AddWithValue("accounting_date", productAccounting.AccountingDate);
                    cmd.Parameters.AddWithValue("quantity", productAccounting.Quantity);
                    cmd.Parameters.AddWithValue("last_movement_date", (object)productAccounting.LastMovementDate ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("movement_status", productAccounting.MovementStatus ?? "В наличии");
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected == 0)
                        throw new KeyNotFoundException($"Запись с идентификатором {productAccounting.ProductAccId} не найдена.");
                }
            }
        }

        /// <summary>
        /// Удаляет запись учета продукта из базы данных по указанному идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор записи учета продукта (productAcc_id).</param>
        /// <exception cref="System.ArgumentException">
        /// Выбрасывается, если параметр <paramref name="id"/> меньше или равен 0.
        /// </exception>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException">
        /// Выбрасывается, если запись с указанным идентификатором не найдена.
        /// </exception>
        public void Delete(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Идентификатор записи учета продукта должен быть больше нуля.", nameof(id));

            using (var conn = _dbConnection.GetConnection())
            {
                conn.Open();

                using (var cmd = new NpgsqlCommand("DELETE FROM product_accounting WHERE productAcc_id = @id", conn))
                {
                    cmd.Parameters.AddWithValue("id", id);
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected == 0)
                        throw new KeyNotFoundException($"Запись с идентификатором {id} не найдена.");
                }
            }
        }

        /// <summary>
        /// Получает отфильтрованный список записей учета продуктов на основе текста поиска.
        /// </summary>
        /// <param name="searchText">Текст для поиска (по идентификатору, поставке, сотруднику, складу, дате или статусу).</param>
        /// <returns>
        /// Список <see cref="List{ProductAccounting}"/> записей, удовлетворяющих условиям поиска.
        /// Если записи не найдены или <paramref name="searchText"/> пустой, возвращает пустой список.
        /// </returns>
        /// <remarks>
        /// <para>
        /// Поиск выполняется без учёта регистра с использованием оператора ILIKE в PostgreSQL.
        /// Поиск охватывает поля таблицы product_accounting, а также связанные поля full_name и position из таблицы employee
        /// и zone_name из таблицы storage_zone.
        /// </para>
        /// <para>
        /// Если параметр <paramref name="searchText"/> равен null или пустой строке, метод возвращает пустой список.
        /// </para>
        /// </remarks>
        /// <example>
        /// <code>
        /// var repository = new ProductAccountingRepository(dbConnection);
        /// var filteredProducts = repository.GetFiltered("warehouse");
        /// foreach (var pa in filteredProducts)
        /// {
        ///     Console.WriteLine($"ID: {pa.ProductAccId}, Quantity: {pa.Quantity}, Status: {pa.MovementStatus}");
        /// }
        /// </code>
        /// </example>
        public List<ProductAccounting> GetFiltered(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
                return new List<ProductAccounting>();

            var productAccountings = new List<ProductAccounting>();
            using (var conn = _dbConnection.GetConnection())
            {
                conn.Open();

                string query = @"
                    SELECT pa.productAcc_id, pa.supply_id, pa.employee_id, pa.storage_id, pa.accounting_date, pa.quantity, pa.last_movement_date, pa.movement_status
                    FROM product_accounting pa
                    LEFT JOIN employee e ON pa.employee_id = e.employee_id
                    LEFT JOIN storage_zone sz ON pa.storage_id = sz.storage_id
                    WHERE pa.productAcc_id::text ILIKE @search
                    OR pa.supply_id::text ILIKE @search
                    OR pa.employee_id::text ILIKE @search
                    OR e.full_name ILIKE @search
                    OR e.position ILIKE @search
                    OR pa.storage_id::text ILIKE @search
                    OR sz.zone_name ILIKE @search
                    OR pa.accounting_date::text ILIKE @search
                    OR pa.quantity::text ILIKE @search
                    OR pa.movement_status ILIKE @search
                    OR (pa.last_movement_date::text ILIKE @search OR pa.last_movement_date IS NULL)";

                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("search", $"%{searchText}%");

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            productAccountings.Add(new ProductAccounting
                            {
                                ProductAccId = reader.GetInt32(0),
                                SupplyId = reader.GetInt32(1),
                                EmployeeId = reader.GetInt32(2),
                                StorageId = reader.GetInt32(3),
                                AccountingDate = reader.GetDateTime(4),
                                Quantity = reader.GetInt32(5),
                                LastMovementDate = reader.IsDBNull(6) ? null : reader.GetDateTime(6),
                                MovementStatus = reader.IsDBNull(7) ? "В наличии" : reader.GetString(7)
                            });
                        }
                    }
                }
            }

            return productAccountings;
        }
    }
}