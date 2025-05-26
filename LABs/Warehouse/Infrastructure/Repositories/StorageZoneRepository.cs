using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Interfaces;
using Domain.Models;
using Npgsql;
using NpgsqlTypes;

namespace Infrastructure.Repositories
{
    public class StorageZoneRepository : IStorageZoneRepository
    {
        private readonly DatabaseConnection _dbConnection;

        public StorageZoneRepository(DatabaseConnection dbConnection)
        {
            _dbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
        }

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

        public StorageZone GetById(int id)
        {
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

        public void Add(StorageZone storageZone)
        {
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

        public void Update(StorageZone storageZone)
        {
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
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            using (var conn = _dbConnection.GetConnection())
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("DELETE FROM storage_zone WHERE storage_id = @id", conn))
                {
                    cmd.Parameters.AddWithValue("id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<StorageZone> GetFiltered(string searchText)
        {
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