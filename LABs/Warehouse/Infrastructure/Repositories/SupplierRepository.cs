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
    public class SupplierRepository : ISupplierRepository
    {
        private readonly DatabaseConnection _dbConnection;

        public SupplierRepository(DatabaseConnection dbConnection)
        {
            _dbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
        }

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