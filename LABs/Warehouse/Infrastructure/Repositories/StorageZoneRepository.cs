using Domain.Interfaces;
using Domain.Models;
using Npgsql;
using NpgsqlTypes;

namespace Infrastructure.Repositories
{
    /// <summary>
    /// Реализация репозитория для работы с данными зон хранения в базе данных PostgreSQL.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Класс реализует интерфейс <see cref="IStorageZoneRepository"/> и предоставляет методы
    /// для выполнения CRUD-операций и фильтрации данных зон хранения с использованием
    /// PostgreSQL через библиотеку Npgsql.
    /// </para>
    /// <para>
    /// Все методы являются атомарными и потокобезопасными, так как используют отдельные
    /// подключения к базе данных через<see cref = "DatabaseConnection" />.
    /// </para>
    /// <para>
    /// Для корректной работы требуется настроенное подключение к базе данных PostgreSQL.
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    /// var dbConnection = new DatabaseConnection("Host=localhost;Username=user;Password=pass;Database=products");
    /// var repository = new StorageZoneRepository(dbConnection);
    /// var storageZones = repository.GetAll();
    /// foreach (var zone in storageZones)
    /// {
    ///     Console.WriteLine($"ID: {zone.StorageId}, Name: {zone.zoneName}, Type: {zone.ZoneType}");
    /// }
    /// </code>
    /// </example>
    public class StorageZoneRepository : IStorageZoneRepository
    {
        private readonly DatabaseConnection _dbConnection;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="StorageZoneRepository"/>.
        /// </summary>
        /// <param name="dbConnection">Объект подключения к базе данных типа <see cref="DatabaseConnection"/>.</param>
        /// <exception cref="System.ArgumentNullException">
        /// Выбрасывается, если параметр <paramref name="dbConnection"/> равен null.
        /// </exception>
        public StorageZoneRepository(DatabaseConnection dbConnection)
        {
            _dbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
        }

        /// <summary>
        /// Получает список всех зон хранения из базы данных.
        /// </summary>
        /// <returns>
        /// Список <see cref="List{StorageZone}"/> всех зон хранения.
        /// Если зоны хранения отсутствуют, возвращает пустой список.
        /// </returns>
        /// <remarks>
        /// Метод выполняет SQL-запрос для получения всех записей из таблицы storage_zone.
        /// </remarks>
        public List<StorageZone> GetAll()
        {
            var storageZones = new List<StorageZone>();
            using (var conn = _dbConnection.GetConnection())
            {
                conn.Open();

                using (var cmd = new NpgsqlCommand("SELECT storage_id, warehouse_id, capacity, zone_type, zone_name FROM storage_zone", conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        storageZones.Add(new StorageZone
                        {
                            StorageId = reader.GetInt32(0),
                            WarehouseId = reader.GetInt32(1),
                            Capacity = reader.GetInt32(2),
                            ZoneType = (ZoneType)Enum.Parse(typeof(ZoneType), reader.GetString(3), true),
                            ZoneName = reader.GetString(4)
                        });
                    }
                }
            }

            return storageZones;
        }

        /// <summary>
        /// Получает зону хранения по указанному идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор зоны хранения (storage_id).</param>
        /// <returns>
        /// Объект <see cref="StorageZone"/> с данными зоны хранения или null, если зона не найдена.
        /// </returns>
        /// <exception cref="System.ArgumentException">
        /// Выбрасывается, если параметр <paramref name="id"/> меньше или равен 0.
        /// </exception>
        public StorageZone GetById(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Идентификатор зоны хранения должен быть больше нуля.", nameof(id));

            using (var conn = _dbConnection.GetConnection())
            {
                conn.Open();

                using (var cmd = new NpgsqlCommand("SELECT storage_id, warehouse_id, capacity, zone_type, zone_name FROM storage_zone WHERE storage_id = @id", conn))
                {
                    cmd.Parameters.AddWithValue("id", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new StorageZone
                            {
                                StorageId = reader.GetInt32(0),
                                WarehouseId = reader.GetInt32(1),
                                Capacity = reader.GetInt32(2),
                                ZoneType = (ZoneType)Enum.Parse(typeof(ZoneType), reader.GetString(3), true),
                                ZoneName = reader.GetString(4)
                            };
                        }

                        return null;
                    }
                }
            }
        }

        /// <summary>
        /// Добавляет новую зону хранения в базу данных.
        /// </summary>
        /// <param name="storageZone">Объект зоны хранения типа <see cref="StorageZone"/>.</param>
        /// <exception cref="System.ArgumentNullException">
        /// Выбрасывается, если параметр <paramref name="storageZone"/> равен null.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// Выбрасывается, если:
        /// - <see cref="StorageZone.WarehouseId"/> меньше или равен 0;
        /// - <see cref="StorageZone.Capacity"/> меньше или равен 0;
        /// - <see cref="StorageZone.ZoneName"/> пустое или содержит только пробелы.
        /// </exception>
        /// <exception cref="System.InvalidOperationException">
        /// Выбрасывается, если зона с таким <see cref="StorageZone.StorageId"/> уже существует.
        /// </exception>
        /// <remarks>
        /// Метод устанавливает свойство <see cref="StorageZone.StorageId"/> на основе возвращаемого значения SQL-запроса.
        /// </remarks>
        public void Add(StorageZone storageZone)
        {
            if (storageZone == null)
                throw new ArgumentNullException(nameof(storageZone));

            if (storageZone.WarehouseId <= 0)
                throw new ArgumentException("Идентификатор склада должен быть больше нуля.", nameof(storageZone.WarehouseId));

            if (storageZone.Capacity <= 0)
                throw new ArgumentException("Вместимость зоны хранения должна быть больше нуля.", nameof(storageZone.Capacity));

            if (string.IsNullOrWhiteSpace(storageZone.ZoneName))
                throw new ArgumentException("Название зоны хранения не может быть пустым.", nameof(storageZone.ZoneName));

            using (var conn = _dbConnection.GetConnection())
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(
                "INSERT INTO storage_zone (warehouse_id, capacity, zone_type, zone_name) VALUES (@warehouse_id, @capacity, @zone_type, @zone_name) RETURNING storage_id", conn))
                {
                    cmd.Parameters.AddWithValue("warehouse_id", storageZone.WarehouseId);
                    cmd.Parameters.AddWithValue("capacity", storageZone.Capacity);
                    cmd.Parameters.Add(new NpgsqlParameter("zone_type", NpgsqlDbType.Unknown) { Value = storageZone.ZoneType.ToString(), DataTypeName = "zone_type" });
                    cmd.Parameters.AddWithValue("zone_name", storageZone.ZoneName);
                    storageZone.StorageId = (int)cmd.ExecuteScalar();
                }
            }
        }

        /// <summary>
        /// Обновляет существующую зону хранения в базе данных.
        /// </summary>
        /// <param name="storageZone">Объект зоны хранения типа <see cref="StorageZone"/> с обновлёнными данными.</param>
        /// <exception cref="System.ArgumentNullException">
        /// Выбрасывается, если параметр <paramref name="storageZone"/> равен null.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// Выбрасывается, если:
        /// - <see cref="StorageZone.StorageId"/> меньше или равен 0;
        /// - <see cref="StorageZone.WarehouseId"/> меньше или равен 0;
        /// - <see cref="StorageZone.Capacity"/> меньше или равен 0;
        /// - <see cref="StorageZone.ZoneName"/> пустое или содержит только пробелы.
        /// </exception>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException">
        /// Выбрасывается, если зона с указанным <see cref="StorageZone.StorageId"/> не найдена.
        /// </exception>
        public void Update(StorageZone storageZone)
        {
            if (storageZone == null)
                throw new ArgumentNullException(nameof(storageZone));

            if (storageZone.StorageId <= 0)
                throw new ArgumentException("Идентификатор зоны хранения должен быть больше нуля.", nameof(storageZone.StorageId));

            if (storageZone.WarehouseId <= 0)
                throw new ArgumentException("Идентификатор склада должен быть больше нуля.", nameof(storageZone.WarehouseId));

            if (storageZone.Capacity <= 0)
                throw new ArgumentException("Вместимость зоны хранения должна быть больше нуля.", nameof(storageZone.Capacity));

            if (string.IsNullOrWhiteSpace(storageZone.ZoneName))
                throw new ArgumentException("Название зоны хранения не может быть пустым.", nameof(storageZone.ZoneName));

            using (var conn = _dbConnection.GetConnection())
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(
                    "UPDATE storage_zone SET warehouse_id = @warehouse_id, capacity = @capacity, zone_type = @zone_type, zone_name = @zone_name WHERE storage_id = @id", conn))
                {
                    cmd.Parameters.AddWithValue("id", storageZone.StorageId);
                    cmd.Parameters.AddWithValue("warehouse_id", storageZone.WarehouseId);
                    cmd.Parameters.AddWithValue("capacity", storageZone.Capacity);
                    cmd.Parameters.Add(new NpgsqlParameter("zone_type", NpgsqlDbType.Unknown) { Value = storageZone.ZoneType.ToString(), DataTypeName = "zone_type" });
                    cmd.Parameters.AddWithValue("zone_name", storageZone.ZoneName);
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected == 0)
                        throw new KeyNotFoundException($"Зона хранения с идентификатором {storageZone.StorageId} не найдена.");
                }
            }
        }

        /// <summary>
        /// Удаляет зону хранения из базы данных по указанному идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор зоны хранения (storage_id).</param>
        /// <exception cref="System.ArgumentException">
        /// Выбрасывается, если параметр <paramref name="id"/> меньше или равен 0.
        /// </exception>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException">
        /// Выбрасывается, если зона с указанным идентификатором не найдена.
        /// </exception>
        public void Delete(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Идентификатор зоны хранения должен быть больше нуля.", nameof(id));

            using (var conn = _dbConnection.GetConnection())
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("DELETE FROM storage_zone WHERE storage_id = @id", conn))
                {
                    cmd.Parameters.AddWithValue("id", id);
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected == 0)
                        throw new KeyNotFoundException($"Зона хранения с идентификатором {id} не найдена.");
                }
            }
        }

        /// <summary>
        /// Получает отфильтрованный список зон хранения на основе текста поиска.
        /// </summary>
        /// <param name="searchText">Текст для поиска (по идентификатору, названию зоны, типу зоны или названию склада).</param>
        /// <returns>
        /// Список <see cref="List{StorageZone}"/> зон хранения, удовлетворяющих условиям поиска.
        /// Если <paramref name="searchText"/> равен null или пустой строке, возвращает пустой список.
        /// </returns>
        /// <remarks>
        /// <para>
        /// Поиск выполняется без учёта регистра с использованием оператора ILIKE в PostgreSQL.
        /// </para>
        /// <para>
        /// Поля, по которым выполняется поиск: storage_id, warehouse_id, capacity, zone_type, zone_name,
        /// а также name из связанной таблицы warehouse.
        /// </para>
        /// </remarks>
        /// <example>
        /// <code>
        /// var repository = new StorageZoneRepository(dbConnection);
        /// var filteredZones = repository.GetFiltered("cold");
        /// foreach (var zone in filteredZones)
        /// {
        ///     Console.WriteLine($"ID: {zone.StorageId}, Name: {zone.ZoneName}, Type: {zone.ZoneType}");
        /// }
        /// </code>
        /// </example>
        public List<StorageZone> GetFiltered(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
                return new List<StorageZone>();

            var storageZones = new List<StorageZone>();
            using (var conn = _dbConnection.GetConnection())
            {
                conn.Open();
                string query = @"
                    SELECT sz.storage_id, sz.warehouse_id, sz.capacity, sz.zone_type, sz.zone_name
                    FROM storage_zone sz
                    LEFT JOIN warehouse w ON sz.warehouse_id = w.warehouse_id
                    WHERE sz.storage_id::text ILIKE @search
                    OR sz.warehouse_id::text ILIKE @search
                    OR sz.capacity::text ILIKE @search
                    OR sz.zone_type::text ILIKE @search
                    OR sz.zone_name ILIKE @search
                    OR w.name ILIKE @search";

                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("search", $"%{searchText}%");
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            storageZones.Add(new StorageZone
                            {
                                StorageId = reader.GetInt32(0),
                                WarehouseId = reader.GetInt32(1),
                                Capacity = reader.GetInt32(2),
                                ZoneType = (ZoneType)Enum.Parse(typeof(ZoneType), reader.GetString(3), true),
                                ZoneName = reader.GetString(4)
                            });
                        }
                    }
                }
            }

            return storageZones;
        }
    }
}