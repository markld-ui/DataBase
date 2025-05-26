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
    public class SupplyRepository : ISupplyRepository
    {
        private readonly DatabaseConnection _dbConnection;

        public SupplyRepository(DatabaseConnection dbConnection)
        {
            _dbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
        }

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