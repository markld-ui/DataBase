using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Interfaces;
using Domain.Models;
using Npgsql;

namespace Infrastructure.Repositories
{
    public class WarehouseRepository : IWarehouseRepository
    {
        private readonly DatabaseConnection _dbConnection;

        public WarehouseRepository(DatabaseConnection dbConnection)
        {
            _dbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
        }

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