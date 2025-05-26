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
    public class ProductAccountingRepository : IProductAccountingRepository
    {
        private readonly DatabaseConnection _dbConnection;

        public ProductAccountingRepository(DatabaseConnection dbConnection)
        {
            _dbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
        }

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

        public ProductAccounting GetById(int id)
        {
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

        public void Add(ProductAccounting productAccounting)
        {
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

        public void Update(ProductAccounting productAccounting)
        {
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
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            using (var conn = _dbConnection.GetConnection())
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("DELETE FROM product_accounting WHERE productAcc_id = @id", conn))
                {
                    cmd.Parameters.AddWithValue("id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<ProductAccounting> GetFiltered(string searchText)
        {
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