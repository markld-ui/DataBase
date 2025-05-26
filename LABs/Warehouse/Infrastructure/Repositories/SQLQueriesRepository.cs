using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Interfaces;
using Npgsql;

namespace Infrastructure.Repositories
{
    public class SQLQueriesRepository : ISQLQueriesRepository
    {
        private readonly DatabaseConnection _dbConnection;

        public SQLQueriesRepository (DatabaseConnection dbConnection)
        {
            _dbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
        }

        public DataTable GetAllRecords()
        {
            using (var conn = _dbConnection.GetConnection())
            {
                conn.Open();
                string query = @"
                    SELECT pa.productAcc_id, pa.accounting_date, pa.quantity, 
                           e.full_name AS employee_name, 
                           sz.zone_name AS storage_zone
                    FROM product_accounting pa
                    INNER JOIN employee e ON pa.employee_id = e.employee_id
                    INNER JOIN storage_zone sz ON pa.storage_id = sz.storage_id";
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    using (var adapter = new NpgsqlDataAdapter(cmd))
                    {
                        DataTable table = new DataTable();
                        adapter.Fill(table);
                        return table;
                    }
                }
            }
        }

        public DataTable GetRecordsByEmployee(int employeeId)
        {
            if (!EmployeeExists(employeeId))
            {
                throw new ArgumentException($"Сотрудник с ID {employeeId} не существует.");
            }

            using (var conn = _dbConnection.GetConnection())
            {
                conn.Open();
                string query = @"
                    SELECT pa.productAcc_id, pa.accounting_date, pa.quantity, 
                           e.full_name AS employee_name, 
                           sz.zone_name AS storage_zone
                    FROM product_accounting pa
                    INNER JOIN employee e ON pa.employee_id = e.employee_id
                    INNER JOIN storage_zone sz ON pa.storage_id = sz.storage_id
                    WHERE pa.employee_id = @employeeId";
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("employeeId", employeeId);
                    using (var adapter = new NpgsqlDataAdapter(cmd))
                    {
                        DataTable table = new DataTable();
                        adapter.Fill(table);
                        return table;
                    }
                }
            }
        }

        public DataTable GetAggregateRecords(int minRecordCount, DateTime startDate)
        {
            if (minRecordCount < 0)
            {
                throw new ArgumentException("Минимальное количество записей не может быть отрицательным.");
            }

            using (var conn = _dbConnection.GetConnection())
            {
                conn.Open();
                string query = @"
                    SELECT sz.zone_name AS storage_zone, 
                           COUNT(pa.productAcc_id) AS record_count, 
                           SUM(pa.quantity) AS total_quantity,
                           AVG(pa.quantity) AS avg_quantity
                    FROM product_accounting pa
                    INNER JOIN storage_zone sz ON pa.storage_id = sz.storage_id
                    WHERE pa.accounting_date >= @startDate
                    GROUP BY sz.zone_name
                    HAVING COUNT(pa.productAcc_id) >= @minRecordCount
                    ORDER BY total_quantity DESC";
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("minRecordCount", minRecordCount);
                    cmd.Parameters.AddWithValue("startDate", startDate);
                    using (var adapter = new NpgsqlDataAdapter(cmd))
                    {
                        DataTable table = new DataTable();
                        adapter.Fill(table);
                        return table;
                    }
                }
            }
        }

        public DataTable GetSimpleProductAccountingRecords()
        {
            using (var conn = _dbConnection.GetConnection())
            {
                conn.Open();
                string query = @"
                    SELECT productAcc_id, accounting_date, quantity, employee_id, supply_id, storage_id
                    FROM product_accounting";
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    using (var adapter = new NpgsqlDataAdapter(cmd))
                    {
                        DataTable table = new DataTable();
                        adapter.Fill(table);
                        return table;
                    }
                }
            }
        }

        public DataTable GetCorrelatedSubquery(int supplyId)
        {
            if (!SupplyExists(supplyId))
            {
                throw new ArgumentException($"Поставка с ID {supplyId} не существует.");
            }

            using (var conn = _dbConnection.GetConnection())
            {
                conn.Open();
                string query = @"
                    SELECT pa.productAcc_id, pa.accounting_date, pa.quantity,
                           (SELECT s.supply_date FROM supply s WHERE s.supply_id = pa.supply_id) AS supply_date,
                           (SELECT e.full_name FROM employee e WHERE e.employee_id = pa.employee_id) AS employee_name
                    FROM product_accounting pa
                    WHERE pa.supply_id = @supplyId";
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("supplyId", supplyId);
                    using (var adapter = new NpgsqlDataAdapter(cmd))
                    {
                        DataTable table = new DataTable();
                        adapter.Fill(table);
                        return table;
                    }
                }
            }
        }

        public DataTable GetNonCorrelatedSubquery(int supplyId)
        {
            if (!SupplyExists(supplyId))
            {
                throw new ArgumentException($"Поставка с ID {supplyId} не существует.");
            }

            using (var conn = _dbConnection.GetConnection())
            {
                conn.Open();
                string query = @"
                    SELECT pa.productAcc_id, pa.accounting_date, pa.quantity,
                           e.full_name AS employee_name
                    FROM product_accounting pa
                    INNER JOIN employee e ON pa.employee_id = e.employee_id
                    WHERE pa.supply_id = @supplyId
                    AND pa.quantity > (SELECT AVG(quantity) FROM product_accounting WHERE supply_id = @supplyId)";
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("supplyId", supplyId);
                    using (var adapter = new NpgsqlDataAdapter(cmd))
                    {
                        DataTable table = new DataTable();
                        adapter.Fill(table);
                        if (table.Rows.Count == 0)
                        {
                            using (var avgCmd = new NpgsqlCommand("SELECT AVG(quantity) FROM product_accounting WHERE supply_id = @supplyId", conn))
                            {
                                avgCmd.Parameters.AddWithValue("supplyId", supplyId);
                                var avg = avgCmd.ExecuteScalar();
                                Console.WriteLine($"DEBUG: Average quantity for supply_id {supplyId} = {avg}");
                            }
                        }
                        return table;
                    }
                }
            }
        }

        public bool EmployeeExists(int employeeId)
        {
            using (var conn = _dbConnection.GetConnection())
            {
                conn.Open();
                string query = "SELECT EXISTS (SELECT 1 FROM employee WHERE employee_id = @employeeId)";
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("employeeId", employeeId);
                    return (bool)cmd.ExecuteScalar();
                }
            }
        }

        public bool SupplyExists(int supplyId)
        {
            using (var conn = _dbConnection.GetConnection())
            {
                conn.Open();
                string query = "SELECT EXISTS (SELECT 1 FROM supply WHERE supply_id = @supplyId)";
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("supplyId", supplyId);
                    return (bool)cmd.ExecuteScalar();
                }
            }
        }

        public bool StorageZoneExists(int storageId)
        {
            using (var conn = _dbConnection.GetConnection())
            {
                conn.Open();
                string query = "SELECT EXISTS (SELECT 1 FROM storage_zone WHERE storage_id = @storageId)";
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("storageId", storageId);
                    return (bool)cmd.ExecuteScalar();
                }
            }
        }

        public bool RecordExists(int recordId)
        {
            using (var conn = _dbConnection.GetConnection())
            {
                conn.Open();
                string query = "SELECT EXISTS (SELECT 1 FROM product_accounting WHERE productAcc_id = @recordId)";
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("recordId", recordId);
                    return (bool)cmd.ExecuteScalar();
                }
            }
        }

        public void InsertRecord(DateTime accountingDate, int quantity, int employeeId, int supplyId, int storageId)
        {
            if (!EmployeeExists(employeeId))
            {
                throw new ArgumentException($"Сотрудник с ID {employeeId} не существует.");
            }
            if (!SupplyExists(supplyId))
            {
                throw new ArgumentException($"Поставка с ID {supplyId} не существует.");
            }
            if (!StorageZoneExists(storageId))
            {
                throw new ArgumentException($"Зона хранения с ID {storageId} не существует.");
            }

            using (var conn = _dbConnection.GetConnection())
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        string query = @"
                            INSERT INTO product_accounting (accounting_date, quantity, employee_id, supply_id, storage_id)
                            VALUES (@accountingDate, @quantity, @employeeId, @supplyId, @storageId)";
                        using (var cmd = new NpgsqlCommand(query, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("accountingDate", accountingDate);
                            cmd.Parameters.AddWithValue("quantity", quantity);
                            cmd.Parameters.AddWithValue("employeeId", employeeId);
                            cmd.Parameters.AddWithValue("supplyId", supplyId);
                            cmd.Parameters.AddWithValue("storageId", storageId);
                            cmd.ExecuteNonQuery();
                        }
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        Console.WriteLine($"ERROR: Failed to insert record. {ex.Message}");
                        throw;
                    }
                }
            }
        }

        public void UpdateRecord(int id, DateTime? accountingDate, int? quantity, int? employeeId, int? supplyId, int? storageId)
        {
            if (employeeId.HasValue && !EmployeeExists(employeeId.Value))
            {
                throw new ArgumentException($"Сотрудник с ID {employeeId.Value} не существует.");
            }
            if (supplyId.HasValue && !SupplyExists(supplyId.Value))
            {
                throw new ArgumentException($"Поставка с ID {supplyId.Value} не существует.");
            }
            if (storageId.HasValue && !StorageZoneExists(storageId.Value))
            {
                throw new ArgumentException($"Зона хранения с ID {storageId.Value} не существует.");
            }
            if (!RecordExists(id))
            {
                throw new ArgumentException($"Запись с ID {id} не существует.");
            }

            using (var conn = _dbConnection.GetConnection())
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        string query = "UPDATE product_accounting SET ";
                        var parameters = new List<NpgsqlParameter>();
                        if (accountingDate.HasValue) { query += "accounting_date = @accountingDate, "; parameters.Add(new NpgsqlParameter("accountingDate", accountingDate.Value)); }
                        if (quantity.HasValue) { query += "quantity = @quantity, "; parameters.Add(new NpgsqlParameter("quantity", quantity.Value)); }
                        if (employeeId.HasValue) { query += "employee_id = @employeeId, "; parameters.Add(new NpgsqlParameter("employeeId", employeeId.Value)); }
                        if (supplyId.HasValue) { query += "supply_id = @supplyId, "; parameters.Add(new NpgsqlParameter("supplyId", supplyId.Value)); }
                        if (storageId.HasValue) { query += "storage_id = @storageId, "; parameters.Add(new NpgsqlParameter("storageId", storageId.Value)); }
                        query = query.TrimEnd(',', ' ') + " WHERE productAcc_id = @id";
                        parameters.Add(new NpgsqlParameter("id", id));

                        using (var cmd = new NpgsqlCommand(query, conn, transaction))
                        {
                            cmd.Parameters.AddRange(parameters.ToArray());
                            cmd.ExecuteNonQuery();
                        }
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        Console.WriteLine($"ERROR: Failed to update record. {ex.Message}");
                        throw;
                    }
                }
            }
        }

        public void DeleteRecord(int id)
        {
            if (!RecordExists(id))
            {
                throw new ArgumentException($"Запись с ID {id} не существует.");
            }

            using (var conn = _dbConnection.GetConnection())
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        string query = "DELETE FROM product_accounting WHERE productAcc_id = @id";
                        using (var cmd = new NpgsqlCommand(query, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("id", id);
                            cmd.ExecuteNonQuery();
                        }
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        Console.WriteLine($"ERROR: Failed to delete record. {ex.Message}");
                        throw;
                    }
                }
            }
        }
    }
}
