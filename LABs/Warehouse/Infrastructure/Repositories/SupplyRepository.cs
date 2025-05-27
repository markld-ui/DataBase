using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using Domain.Interfaces;
using Domain.Models;

namespace Infrastructure.Repositories
{
    /// <summary>
    /// Реализация репозитория для работы с данными поставок в базе данных PostgreSQL.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Класс реализует интерфейс <see cref="ISupplyRepository"/> и предоставляет методы
    /// для выполнения CRUD-операций и фильтрации данных поставок с использованием
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
    /// var repository = new SupplyRepository(dbConnection);
    /// var supplies = repository.GetAll();
    /// foreach (var supply in supplies)
    /// {
    ///     Console.WriteLine($"ID: {supply.SupplyId}, Product ID: {supply.ProductId}, Date: {supply.SupplyDate}");
    /// }
    /// </code>
    /// </example>
    public class SupplyRepository : ISupplyRepository
    {
        private readonly DatabaseConnection _dbConnection;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="SupplierRepository"/>.</summary>
        /// <param name="dbConnection">Объект подключения к базе данных типа <see cref="DatabaseConnection"/>.</param>
        /// <exception cref="System.ArgumentNullException">
        /// Выбрасывается, если параметр <paramref name="dbConnection"/> равен null.
        /// </exception>
        public SupplyRepository(DatabaseConnection dbConnection)
        {
            _dbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
        }

        /// <summary>
        /// Получает список всех поставок из базы данных.
        /// </summary>
        /// <returns>
        /// Список объектов <see cref="List{Supply}"/> всех поставок.
        /// List Возвращает пустой список, если поставки отсутствуют.
        /// </returns>
        /// <remarks>
        /// Метод выполняет SQL-запрос для получения всех записей из таблицы supply.
        /// </remarks>
        public List<Supply> GetAll()
        {
            var supplies = new List<Supply>();
            using (var conn = _dbConnection.GetConnection())
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("SELECT supply_id, product_id, supplier_id, supply_date, quantity FROM supply", conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        supplies.Add(new Supply
                        {
                            SupplyId = reader.GetInt32(0),
                            ProductId = reader.GetInt32(1),
                            SupplierId = reader.GetInt32(2),
                            SupplyDate = reader.GetDateTime(3),
                            Quantity = reader.GetInt32(4)
                        });
                    }
                }
            }
            return supplies;
        }

        /// <summary>
        /// Получает данные поставки по указанному идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор поставки (положительное число).</param>
        /// <returns>
        /// Объект <see cref="Supply"/> с данными или поставки null, если поставка не найдена.
        /// </returns>
        public Supply GetById(int id)
        {
            using (var conn = _dbConnection.GetConnection())
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("SELECT supply_id, product_id, supplier_id, supply_date, quantity FROM supply WHERE supply_id = @id", conn))
                {
                    cmd.Parameters.AddWithValue("id", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Supply
                            {
                                SupplyId = reader.GetInt32(0),
                                ProductId = reader.GetInt32(1),
                                SupplierId = reader.GetInt32(2),
                                SupplyDate = reader.GetDateTime(3),
                                Quantity = reader.GetInt32(4)
                            };
                        }
                        return null;
                    }
                }
            }
        }

        /// <summary>
        /// Добавляет новую поставку в базу данных.
        /// </summary>
        /// <param name="supply">Объект поставки типа <see cref="Supply"/>.</param>
        /// <remarks>
        /// Метод устанавливает свойство <see cref="Supply.SupplyId"/> на основе
        /// возвращаемого значения SQL-запроса.
        /// </remarks>
        public void Add(Supply supply)
        {
            using (var conn = _dbConnection.GetConnection())
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(
                    "INSERT INTO supply (product_id, supplier_id, supply_date, quantity) VALUES (@product_id, @supplier_id, @supply_date, @quantity) RETURNING supply_id", conn))
                {
                    cmd.Parameters.AddWithValue("product_id", supply.ProductId);
                    cmd.Parameters.AddWithValue("supplier_id", supply.SupplierId);
                    cmd.Parameters.AddWithValue("supply_date", supply.SupplyDate);
                    cmd.Parameters.AddWithValue("quantity", supply.Quantity);
                    supply.SupplyId = (int)cmd.ExecuteScalar();
                }
            }
        }

        /// <summary>
        /// Обновляет данные существующей поставки в базе данных.
        /// </summary>
        /// <param name="supply">Объект поставки с обновлёнными данными типа <see cref="Supply"/>.</param>
        public void Update(Supply supply)
        {
            using (var conn = _dbConnection.GetConnection())
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(
                    "UPDATE supply SET product_id = @product_id, supplier_id = @supplier_id, supply_date = @supply_date, quantity = @quantity WHERE supply_id = @id", conn))
                {
                    cmd.Parameters.AddWithValue("id", supply.SupplyId);
                    cmd.Parameters.AddWithValue("product_id", supply.ProductId);
                    cmd.Parameters.AddWithValue("supplier_id", supply.SupplierId);
                    cmd.Parameters.AddWithValue("supply_date", supply.SupplyDate);
                    cmd.Parameters.AddWithValue("quantity", supply.Quantity);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Удаляет поставку из базы данных по указанному идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор поставки (положительное число).</param>
        public void Delete(int id)
        {
            using (var conn = _dbConnection.GetConnection())
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("DELETE FROM supply WHERE supply_id = @id", conn))
                {
                    cmd.Parameters.AddWithValue("id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Получает отфильтрованный список поставок на основе текста поиска.
        /// </summary>
        /// <param name="searchText">Текст для поиска по полям поставки (идентификатор, продукт, поставщик, дата поставки, количество, название продукта, название компании).</param>
        /// <returns>
        /// Список объектов <see cref="List{Supply}"/> поставок, соответствующих критериям поиска.
        /// Возвращает пустой список, если ничего не найдено.
        /// </returns>
        /// <remarks>
        /// <para>
        /// Поиск выполняется без учёта регистра с использованием оператора ILIKE в PostgreSQL.
        /// </para>
        /// <para>
        /// Поля, по которым выполняется поиск: supply_id, product_id, supplier_id, supply_date, quantity,
        /// а также name из таблицы product и company_name из таблицы supplier.
        /// </para>
        /// </remarks>
        /// <example>
        /// <code>
        /// var repository = new SupplyRepository(dbConnection);
        /// var filteredSupplies = repository.GetFiltered("2023");
        /// foreach (var supply in filteredSupplies)
        /// {
        ///     Console.WriteLine($"ID: {supply.SupplyId}, Product ID: {supply.ProductId}, Date: {supply.SupplyDate}");
        /// }
        /// </code>
        /// </example>
        public List<Supply> GetFiltered(string searchText)
        {
            var supplies = new List<Supply>();
            using (var conn = _dbConnection.GetConnection())
            {
                conn.Open();
                string query = @"
                    SELECT s.supply_id, s.product_id, s.supplier_id, s.supply_date, s.quantity
                    FROM supply s
                    LEFT JOIN product p ON s.product_id = p.product_id
                    LEFT JOIN supplier sup ON s.supplier_id = sup.supplier_id
                    WHERE s.supply_id::text ILIKE @search
                    OR s.product_id::text ILIKE @search
                    OR s.supplier_id::text ILIKE @search
                    OR s.supply_date::text ILIKE @search
                    OR s.quantity::text ILIKE @search
                    OR p.name ILIKE @search
                    OR sup.company_name ILIKE @search";
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("search", $"%{searchText}%");
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            supplies.Add(new Supply
                            {
                                SupplyId = reader.GetInt32(0),
                                ProductId = reader.GetInt32(1),
                                SupplierId = reader.GetInt32(2),
                                SupplyDate = reader.GetDateTime(3),
                                Quantity = reader.GetInt32(4)
                            });
                        }
                    }
                }
            }
            return supplies;
        }
    }
}