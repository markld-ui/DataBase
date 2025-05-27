using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Interfaces;
using Domain.Models;
using NpgsqlTypes;

namespace Infrastructure.Repositories
{
    /// <summary>
    /// Реализация репозитория для работы с данными продуктов в базе данных PostgreSQL.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Класс реализует интерфейс <see cref="IProductRepository"/> и предоставляет методы для выполнения
    /// CRUD-операций и фильтрации данных продуктов с использованием PostgreSQL через библиотеку Npgsql.
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
    /// var repository = new ProductRepository(dbConnection);
    /// var products = repository.GetAll();
    /// foreach (var product in products)
    /// {
    ///     Console.WriteLine($"ID: {product.ProductId}, Name: {product.Name}, Type: {product.ProductType}");
    /// }
    /// </code>
    /// </example>
    public class ProductRepository : IProductRepository
    {
        private readonly DatabaseConnection _dbConnection;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ProductRepository"/>.
        /// </summary>
        /// <param name="dbConnection">Объект подключения к базе данных типа <see cref="DatabaseConnection"/>.</param>
        /// <exception cref="System.ArgumentNullException">
        /// Выбрасывается, если параметр <paramref name="dbConnection"/> равен null.
        /// </exception>
        public ProductRepository(DatabaseConnection dbConnection)
        {
            _dbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
        }

        /// <summary>
        /// Получает список всех продуктов из базы данных.
        /// </summary>
        /// <returns>
        /// Список <see cref="List{Product}"/> всех продуктов.
        /// Если продукты отсутствуют, возвращает пустой список.
        /// </returns>
        /// <remarks>
        /// Метод выполняет SQL-запрос для получения всех записей из таблицы product.
        /// </remarks>
        public List<Product> GetAll()
        {
            var products = new List<Product>();
            using (var conn = _dbConnection.GetConnection())
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("SELECT product_id, name, expiry_date, product_type, is_active, photo FROM product", conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        products.Add(new Product
                        {
                            ProductId = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            ExpiryDate = reader.IsDBNull(2) ? null : reader.GetDateTime(2),
                            ProductType = (ProductType)Enum.Parse(typeof(ProductType), reader.GetString(3), true),
                            IsActive = reader.GetBoolean(4),
                            Photo = reader.IsDBNull(5) ? null : (byte[])reader.GetValue(5)
                        });
                    }
                }
            }
            return products;
        }

        /// <summary>
        /// Получает продукт по указанному идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор продукта (product_id).</param>
        /// <returns>
        /// Объект <see cref="Product"/> с данными продукта или null, если продукт не найден.
        /// </returns>
        /// <exception cref="System.ArgumentException">
        /// Выбрасывается, если параметр <paramref name="id"/> меньше или равен 0.
        /// </exception>
        public Product GetById(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Идентификатор продукта должен быть больше нуля.", nameof(id));

            using (var conn = _dbConnection.GetConnection())
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("SELECT product_id, name, expiry_date, product_type, is_active, photo FROM product WHERE product_id = @id", conn))
                {
                    cmd.Parameters.AddWithValue("id", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Product
                            {
                                ProductId = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                ExpiryDate = reader.IsDBNull(2) ? null : reader.GetDateTime(2),
                                ProductType = (ProductType)Enum.Parse(typeof(ProductType), reader.GetString(3), true),
                                IsActive = reader.GetBoolean(4),
                                Photo = reader.IsDBNull(5) ? null : (byte[])reader.GetValue(5)
                            };
                        }
                        return null;
                    }
                }
            }
        }

        /// <summary>
        /// Добавляет новый продукт в базу данных.
        /// </summary>
        /// <param name="product">Объект продукта типа <see cref="Product"/>.</param>
        /// <exception cref="System.ArgumentNullException">
        /// Выбрасывается, если параметр <paramref name="product"/> равен null.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// Выбран, если поле <see cref="Product.Name"/> пустое или содержит только пробелы.
        /// </exception>
        /// <exception cref="System.InvalidOperationException">
        /// Выбрасывается, если продукт с таким <see cref="Product.ProductId"/> уже существует.
        /// </exception>
        /// <remarks>
        /// Метод устанавливает свойство <see cref="Product.ProductId"/> на основе возвращаемого значения SQL-запроса.
        /// </remarks>
        public void Add(Product product)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));
            if (string.IsNullOrWhiteSpace(product.Name))
                throw new ArgumentException("Название продукта не может быть пустым.", nameof(product.Name));

            using (var conn = _dbConnection.GetConnection())
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(
                    "INSERT INTO product (name, expiry_date, product_type, is_active, photo) VALUES (@name, @expiry_date, @product_type, @is_active, @photo) RETURNING product_id", conn))
                {
                    cmd.Parameters.AddWithValue("name", product.Name);
                    cmd.Parameters.AddWithValue("expiry_date", (object)product.ExpiryDate ?? DBNull.Value);
                    cmd.Parameters.Add(new NpgsqlParameter("product_type", NpgsqlDbType.Unknown) { Value = product.ProductType.ToString(), DataTypeName = "product_type" });
                    cmd.Parameters.AddWithValue("is_active", product.IsActive);
                    cmd.Parameters.AddWithValue("photo", (object)product.Photo ?? DBNull.Value);
                    product.ProductId = (int)cmd.ExecuteScalar();
                }
            }
        }

        /// <summary>
        /// Обновляет существующий продукт в базе данных.
        /// </summary>
        /// <param name="product">Объект продукта типа <see cref="Product"/> с обновлёнными данными.</param>
        /// <exception cref="System.ArgumentNullException">
        /// Выбрасывается, если параметр <paramref name="product"/> равен null.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// Выбрасывается, если <see cref="Product.ProductId"/> меньше или равен 0, либо если
        /// поле <see cref="Product.Name"/> пустое или содержит только пробелы.
        /// </exception>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException">
        /// Выбрасывается, если продукт с указанным <see cref="Product.ProductId"/> не найден.
        /// </exception>
        public void Update(Product product)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));
            if (product.ProductId <= 0)
                throw new ArgumentException("Идентификатор продукта должен быть больше нуля.", nameof(product.ProductId));
            if (string.IsNullOrWhiteSpace(product.Name))
                throw new ArgumentException("Название продукта не может быть пустым.", nameof(product.Name));

            using (var conn = _dbConnection.GetConnection())
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(
                    "UPDATE product SET name = @name, expiry_date = @expiry_date, product_type = @product_type, is_active = @is_active, photo = @photo WHERE product_id = @id", conn))
                {
                    cmd.Parameters.AddWithValue("id", product.ProductId);
                    cmd.Parameters.AddWithValue("name", product.Name);
                    cmd.Parameters.AddWithValue("expiry_date", (object)product.ExpiryDate ?? DBNull.Value);
                    cmd.Parameters.Add(new NpgsqlParameter("product_type", NpgsqlDbType.Unknown) { Value = product.ProductType.ToString(), DataTypeName = "product_type" });
                    cmd.Parameters.AddWithValue("is_active", product.IsActive);
                    cmd.Parameters.AddWithValue("photo", (object)product.Photo ?? DBNull.Value);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected == 0)
                        throw new KeyNotFoundException($"Продукт с идентификатором {product.ProductId} не найден.");
                }
            }
        }

        /// <summary>
        /// Удаляет продукт из базы данных по указанному идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор продукта (product_id).</param>
        /// <exception cref="System.ArgumentException">
        /// Выбрасывается, если параметр <paramref name="id"/> меньше или равен 0.
        /// </exception>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException">
        /// Выбрасывается, если продукт с указанным идентификатором не найден.
        /// </exception>
        public void Delete(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Идентификатор продукта должен быть больше нуля.", nameof(id));

            using (var conn = _dbConnection.GetConnection())
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("DELETE FROM product WHERE product_id = @id", conn))
                {
                    cmd.Parameters.AddWithValue("id", id);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected == 0)
                        throw new KeyNotFoundException($"Продукт с идентификатором {id} не найден.");
                }
            }
        }

        /// <summary>
        /// Получает отфильтрованный список продуктов на основе текста поиска.
        /// </summary>
        /// <param name="searchText">Текст для поиска (по идентификатору, названию, типу или дате истечения срока годности).</param>
        /// <returns>
        /// Список <see cref="List{Product}"/> продуктов, удовлетворяющих условиям поиска.
        /// Если <paramref name="searchText"/> равен null или пустой строке, возвращает все продукты.
        /// </returns>
        /// <remarks>
        /// <para>
        /// Поиск выполняется без учёта регистра с использованием оператора ILIKE в PostgreSQL.
        /// </para>
        /// <para>
        /// Поля, по которым выполняется поиск: product_id, name, expiry_date, product_type, is_active.
        /// </para>
        /// </remarks>
        /// <example>
        /// <code>
        /// var repository = new ProductRepository(dbConnection);
        /// var filteredProducts = repository.GetFiltered("food");
        /// foreach (var product in filteredProducts)
        /// {
        ///     Console.WriteLine($"ID: {product.ProductId}, Name: {product.Name}, Type: {product.ProductType}");
        /// }
        /// </code>
        /// </example>
        public List<Product> GetFiltered(string searchText)
        {
            var products = new List<Product>();
            using (var conn = _dbConnection.GetConnection())
            {
                conn.Open();
                string query;
                if (string.IsNullOrWhiteSpace(searchText))
                {
                    query = "SELECT product_id, name, expiry_date, product_type, is_active, photo FROM product";
                }
                else
                {
                    query = @"
                        SELECT product_id, name, expiry_date, product_type, is_active, photo
                        FROM product
                        WHERE product_id::text ILIKE @search
                        OR name ILIKE @search
                        OR expiry_date::text ILIKE @search
                        OR product_type::text ILIKE @search
                        OR is_active::text ILIKE @search";
                }
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    if (!string.IsNullOrWhiteSpace(searchText))
                        cmd.Parameters.AddWithValue("search", $"%{searchText}%");
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            products.Add(new Product
                            {
                                ProductId = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                ExpiryDate = reader.IsDBNull(2) ? null : reader.GetDateTime(2),
                                ProductType = (ProductType)Enum.Parse(typeof(ProductType), reader.GetString(3), true),
                                IsActive = reader.GetBoolean(4),
                                Photo = reader.IsDBNull(5) ? null : (byte[])reader.GetValue(5)
                            });
                        }
                    }
                }
            }
            return products;
        }
    }
}