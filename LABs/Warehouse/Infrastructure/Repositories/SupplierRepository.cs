using Npgsql;
using System;
using Domain.Interfaces;
using Domain.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    /// <summary>
    /// Реализация репозитория для работы с данными поставщиков в базе данных PostgreSQL.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Класс реализует интерфейс <see cref="ISupplierRepository"/> и предоставляет методы
    /// для выполнения CRUD-операций и фильтрации данных поставщиков с использованием
    /// библиотеки Npgsql для взаимодействия с PostgreSQL.
    /// </para>
    /// <para>
    /// Все методы используют параметризованные запросы для защиты от SQL-инъекций
    /// и являются потокобезопасными благодаря независимым подключениям через <see cref="DatabaseConnection"/>.
    /// </para>
    /// <para>
    /// Для работы требуется настроенное подключение к базе данных PostgreSQL.
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    /// var dbConnection = new DatabaseConnection("Host=localhost;Username=user;Password=pass;Database=products");
    /// var repository = new SupplierRepository(dbConnection);
    /// var suppliers = repository.GetAll();
    /// foreach (var supplier in suppliers)
    /// {
    ///     Console.WriteLine($"ID: {supplier.SupplierId}, Company: {supplier.CompanyName}");
    /// }
    /// </code>
    /// </example>
    public class SupplierRepository : ISupplierRepository
    {
        private readonly DatabaseConnection _dbConnection;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="SupplierRepository"/>.
        /// </summary>
        /// <param name="dbConnection">Объект подключения к базе данных типа <see cref="DatabaseConnection"/>.</param>
        /// <exception cref="System.ArgumentNullException">
        /// Выбрасывается, если параметр <paramref name="dbConnection"/> равен null.
        /// </exception>
        public SupplierRepository(DatabaseConnection dbConnection)
        {
            _dbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
        }

        /// <summary>
        /// Получает список всех поставщиков из базы данных.
        /// </summary>
        /// <returns>
        /// Список объектов <see cref="List{Supplier}"/> всех поставщиков.
        /// Возвращает пустой список, если поставщики отсутствуют.
        /// </returns>
        /// <remarks>
        /// Метод выполняет SQL-запрос для получения всех записей из таблицы supplier.
        /// Поля contact_person, phone и address могут быть null.
        /// </remarks>
        public List<Supplier> GetAll()
        {
            var suppliers = new List<Supplier>();
            using (var conn = _dbConnection.GetConnection())
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("SELECT supplier_id, company_name, contact_person, phone, address FROM supplier", conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        suppliers.Add(new Supplier
                        {
                            SupplierId = reader.GetInt32(0),
                            CompanyName = reader.GetString(1),
                            ContactPerson = reader.IsDBNull(2) ? null : reader.GetString(2),
                            Phone = reader.IsDBNull(3) ? null : reader.GetString(3),
                            Address = reader.IsDBNull(4) ? null : reader.GetString(4)
                        });
                    }
                }
            }

            return suppliers;
        }

        /// <summary>
        /// Получает данные поставщика по указанному идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор поставщика (положительное число).</param>
        /// <returns>
        /// Объект <see cref="Supplier"/> с данными поставщика или null, если поставщик не найден.
        /// </returns>
        /// <remarks>
        /// Поля contact_person, phone и address могут быть null.
        /// </remarks>
        public Supplier GetById(int id)
        {
            using (var conn = _dbConnection.GetConnection())
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("SELECT supplier_id, company_name, contact_person, phone, address FROM supplier WHERE supplier_id = @id", conn))
                {
                    cmd.Parameters.AddWithValue("id", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Supplier
                            {
                                SupplierId = reader.GetInt32(0),
                                CompanyName = reader.GetString(1),
                                ContactPerson = reader.IsDBNull(2) ? null : reader.GetString(2),
                                Phone = reader.IsDBNull(3) ? null : reader.GetString(3),
                                Address = reader.IsDBNull(4) ? null : reader.GetString(4)
                            };
                        }

                        return null;
                    }
                }
            }
        }

        /// <summary>
        /// Добавляет нового поставщика в базу данных.
        /// </summary>
        /// <param name="supplier">Объект поставщика типа <see cref="Supplier"/>.</param>
        /// <remarks>
        /// Метод устанавливает свойство <see cref="Supplier.SupplierId"/> на основе
        /// возвращаемого значения SQL-запроса. Поля contact_person, phone и address
        /// могут быть null.
        /// </remarks>
        public void Add(Supplier supplier)
        {
            using (var conn = _dbConnection.GetConnection())
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(
                    "INSERT INTO supplier (company_name, contact_person, phone, address) VALUES (@company_name, @contact_person, @phone, @address) RETURNING supplier_id", conn))
                {
                    cmd.Parameters.AddWithValue("company_name", supplier.CompanyName);
                    cmd.Parameters.AddWithValue("contact_person", (object)supplier.ContactPerson ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("phone", (object)supplier.Phone ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("address", (object)supplier.Address ?? DBNull.Value);
                    supplier.SupplierId = (int)cmd.ExecuteScalar();
                }
            }
        }

        /// <summary>
        /// Обновляет данные существующего поставщика в базе данных.
        /// </summary>
        /// <param name="supplier">Объект поставщика типа <see cref="Supplier"/> с обновлёнными данными.</param>
        /// <remarks>
        /// Поля contact_person, phone и address могут быть null.
        /// </remarks>
        public void Update(Supplier supplier)
        {
            using (var conn = _dbConnection.GetConnection())
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(
                    "UPDATE supplier SET company_name = @company_name, contact_person = @contact_person, phone = @phone, address = @address WHERE supplier_id = @id", conn))
                {
                    cmd.Parameters.AddWithValue("id", supplier.SupplierId);
                    cmd.Parameters.AddWithValue("company_name", supplier.CompanyName);
                    cmd.Parameters.AddWithValue("contact_person", (object)supplier.ContactPerson ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("phone", (object)supplier.Phone ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("address", (object)supplier.Address ?? DBNull.Value);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Удаляет поставщика из базы данных по указанному идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор поставщика (положительное число).</param>
        public void Delete(int id)
        {
            using (var conn = _dbConnection.GetConnection())
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("DELETE FROM supplier WHERE supplier_id = @id", conn))
                {
                    cmd.Parameters.AddWithValue("id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Получает отфильтрованный список поставщиков на основе текста поиска.
        /// </summary>
        /// <param name="searchText">Текст для поиска по полям поставщика (идентификатор, название компании, контактное лицо, телефон, адрес).</param>
        /// <returns>
        /// Список объектов <see cref="List{Supplier}"/> поставщиков, соответствующих критериям поиска.
        /// Возвращает пустой список, если ничего не найдено.
        /// </returns>
        /// <remarks>
        /// <para>
        /// Поиск выполняется без учёта регистра с использованием оператора ILIKE в PostgreSQL.
        /// </para>
        /// <para>
        /// Поля, по которым выполняется поиск: supplier_id, company_name, contact_person, phone, address.
        /// Поля contact_person, phone и address могут быть null.
        /// </para>
        /// </remarks>
        /// <example>
        /// <code>
        /// var repository = new SupplierRepository(dbConnection);
        /// var filteredSuppliers = repository.GetFiltered("Acme");
        /// foreach (var supplier in filteredSuppliers)
        /// {
        ///     Console.WriteLine($"ID: {supplier.SupplierId}, Company: {supplier.CompanyName}");
        /// }
        /// </code>
        /// </example>
        public List<Supplier> GetFiltered(string searchText)
        {
            var suppliers = new List<Supplier>();
            using (var conn = _dbConnection.GetConnection())
            {
                conn.Open();
                string query = @"
                    SELECT supplier_id, company_name, contact_person, phone, address
                    FROM supplier
                    WHERE supplier_id::text ILIKE @search
                    OR company_name ILIKE @search
                    OR contact_person ILIKE @search
                    OR phone ILIKE @search
                    OR address ILIKE @search";

                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("search", $"%{searchText}%");
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            suppliers.Add(new Supplier
                            {
                                SupplierId = reader.GetInt32(0),
                                CompanyName = reader.GetString(1),
                                ContactPerson = reader.IsDBNull(2) ? null : reader.GetString(2),
                                Phone = reader.IsDBNull(3) ? null : reader.GetString(3),
                                Address = reader.IsDBNull(4) ? null : reader.GetString(4)
                            });
                        }
                    }
                }
            }

            return suppliers;
        }
    }
}