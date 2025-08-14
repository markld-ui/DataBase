using Domain.Interfaces;
using Domain.Models;
using Npgsql;

namespace Infrastructure.Repositories
{
    /// <summary>
    /// Реализация репозитория для работы с данными складов в базе данных PostgreSQL.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Класс реализует интерфейс <see cref="IWarehouseRepository"/> и предоставляет методы
    /// для выполнения CRUD-операций и фильтрации данных складов с использованием
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
    /// var repository = new WarehouseRepository(dbConnection);
    /// var warehouses = repository.GetAll();
    /// foreach (var warehouse in warehouses)
    /// {
    ///     Console.WriteLine($"ID: {warehouse.WarehouseId}, Name: {warehouse.Name}, Address: {warehouse.Address}");
    /// }
    /// </code>
    /// </example>
    public class WarehouseRepository : IWarehouseRepository
    {
        private readonly DatabaseConnection _dbConnection;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="WarehouseRepository"/>.
        /// </summary>
        /// <param name="dbConnection">Объект подключения к базе данных типа <see cref="DatabaseConnection"/>.</param>
        /// <exception cref="System.ArgumentNullException">
        /// Выбрасывается, если параметр <paramref name="dbConnection"/> равен null.
        /// </exception>
        public WarehouseRepository(DatabaseConnection dbConnection)
        {
            _dbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
        }

        /// <summary>
        /// Получает список всех складов из базы данных.
        /// </summary>
        /// <returns>
        /// Список объектов <see cref="List{Warehouse}"/> всех складов.
        /// Возвращает пустой список, если склады отсутствуют.
        /// </returns>
        /// <remarks>
        /// Метод выполняет SQL-запрос для получения всех записей из таблицы warehouse.
        /// </remarks>
        public List<Warehouse> GetAll()
        {
            var warehouses = new List<Warehouse>();
            using (var conn = _dbConnection.GetConnection())
            {
                conn.Open();

                using (var cmd = new NpgsqlCommand("SELECT warehouse_id, name, address FROM warehouse", conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        warehouses.Add(new Warehouse
                        {
                            WarehouseId = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Address = reader.GetString(2)
                        });
                    }
                }
            }

            return warehouses;
        }

        /// <summary>
        /// Получает данные склада по указанному идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор склада (положительное число).</param>
        /// <returns>
        /// Объект <see cref="Warehouse"/> с данными склада или null, если склад не найден.
        /// </returns>
        public Warehouse GetById(int id)
        {
            using (var conn = _dbConnection.GetConnection())
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("SELECT warehouse_id, name, address FROM warehouse WHERE warehouse_id = @id", conn))
                {
                    cmd.Parameters.AddWithValue("id", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Warehouse
                            {
                                WarehouseId = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Address = reader.GetString(2)
                            };
                        }

                        return null;
                    }
                }
            }
        }

        /// <summary>
        /// Добавляет новый склад в базу данных.
        /// </summary>
        /// <param name="warehouse">Объект склада типа <see cref="Warehouse"/>.</param>
        /// <remarks>
        /// Метод устанавливает свойство <see cref="Warehouse.WarehouseId"/> на основе
        /// возвращаемого значения SQL-запроса.
        /// </remarks>
        public void Add(Warehouse warehouse)
        {
            using (var conn = _dbConnection.GetConnection())
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(
                    "INSERT INTO warehouse (name, address) VALUES (@name, @address) RETURNING warehouse_id", conn))
                {
                    cmd.Parameters.AddWithValue("name", warehouse.Name);
                    cmd.Parameters.AddWithValue("address", warehouse.Address);
                    warehouse.WarehouseId = (int)cmd.ExecuteScalar();
                }
            }
        }

        /// <summary>
        /// Обновляет данные существующего склада в базе данных.
        /// </summary>
        /// <param name="warehouse">Объект склада с обновлёнными данными типа <see cref="Warehouse"/>.</param>
        public void Update(Warehouse warehouse)
        {
            using (var conn = _dbConnection.GetConnection())
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(
                    "UPDATE warehouse SET name = @name, address = @address WHERE warehouse_id = @id", conn))
                {
                    cmd.Parameters.AddWithValue("id", warehouse.WarehouseId);
                    cmd.Parameters.AddWithValue("name", warehouse.Name);
                    cmd.Parameters.AddWithValue("address", warehouse.Address);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Удаляет склад из базы данных по указанному идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор склада (положительное число).</param>
        public void Delete(int id)
        {
            using (var conn = _dbConnection.GetConnection())
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("DELETE FROM warehouse WHERE warehouse_id = @id", conn))
                {
                    cmd.Parameters.AddWithValue("id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Получает отфильтрованный список складов на основе текста поиска.
        /// </summary>
        /// <param name="searchText">Текст для поиска по полям склада (идентификатор, название, адрес).</param>
        /// <returns>
        /// Список объектов <see cref="List{Warehouse}"/> складов, соответствующих критериям поиска.
        /// Возвращает пустой список, если ничего не найдено.
        /// </returns>
        /// <remarks>
        /// Поиск выполняется без учёта регистра с использованием оператора ILIKE в PostgreSQL.
        /// </remarks>
        /// <example>
        /// <code>
        /// var repository = new WarehouseRepository(dbConnection);
        /// var filteredWarehouses = repository.GetFiltered("Main");
        /// foreach (var warehouse in filteredWarehouses)
        /// {
        ///     Console.WriteLine($"ID: {warehouse.WarehouseId}, Name: {warehouse.Name}, Address: {warehouse.Address}");
        /// }
        /// </code>
        /// </example>
        public List<Warehouse> GetFiltered(string searchText)
        {
            var warehouses = new List<Warehouse>();
            using (var conn = _dbConnection.GetConnection())
            {
                conn.Open();
                string query = "SELECT warehouse_id, name, address FROM warehouse WHERE " +
                               "warehouse_id::text ILIKE @search OR name ILIKE @search OR address ILIKE @search";

                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("search", $"%{searchText}%");
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            warehouses.Add(new Warehouse
                            {
                                WarehouseId = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Address = reader.GetString(2)
                            });
                        }
                    }
                }
            }

            return warehouses;
        }
    }
}