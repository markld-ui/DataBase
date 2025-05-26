using System;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Interfaces;
using Domain.Models;

namespace Infrastructure.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly DatabaseConnection _dbConnection;

        public EmployeeRepository(DatabaseConnection dbConnection)
        {
            _dbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
        }

        public List<Employee> GetAll()
        {
            var employees = new List<Employee>();
            using (var conn = _dbConnection.GetConnection())
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("SELECT employee_id, photo, full_name, position, phone FROM employee", conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        employees.Add(new Employee
                        {
                            EmployeeId = reader.GetInt32(0),
                            Photo = reader.IsDBNull(1) ? null : (byte[])reader.GetValue(1),
                            FullName = reader.GetString(2),
                            Position = reader.GetString(3),
                            Phone = reader.IsDBNull(4) ? null : reader.GetString(4)
                        });
                    }
                }
            }
            return employees;
        }

        public Employee GetById(int id)
        {
            using (var conn = _dbConnection.GetConnection())
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("SELECT employee_id, photo, full_name, position, phone FROM employee WHERE employee_id = @id", conn))
                {
                    cmd.Parameters.AddWithValue("id", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Employee
                            {
                                EmployeeId = reader.GetInt32(0),
                                Photo = reader.IsDBNull(1) ? null : (byte[])reader.GetValue(1),
                                FullName = reader.GetString(2),
                                Position = reader.GetString(3),
                                Phone = reader.IsDBNull(4) ? null : reader.GetString(4)
                            };
                        }
                        return null;
                    }
                }
            }
        }

        public void Add(Employee employee)
        {
            using (var conn = _dbConnection.GetConnection())
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(
                    "INSERT INTO employee (photo, full_name, position, phone) VALUES (@photo, @full_name, @position, @phone) RETURNING employee_id", conn))
                {
                    cmd.Parameters.AddWithValue("photo", (object)employee.Photo ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("full_name", employee.FullName);
                    cmd.Parameters.AddWithValue("position", employee.Position);
                    cmd.Parameters.AddWithValue("phone", (object)employee.Phone ?? DBNull.Value);
                    employee.EmployeeId = (int)cmd.ExecuteScalar();
                }
            }
        }

        public void Update(Employee employee)
        {
            using (var conn = _dbConnection.GetConnection())
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(
                    "UPDATE employee SET photo = @photo, full_name = @full_name, position = @position, phone = @phone WHERE employee_id = @id", conn))
                {
                    cmd.Parameters.AddWithValue("id", employee.EmployeeId);
                    cmd.Parameters.AddWithValue("photo", (object)employee.Photo ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("full_name", employee.FullName);
                    cmd.Parameters.AddWithValue("position", employee.Position);
                    cmd.Parameters.AddWithValue("phone", (object)employee.Phone ?? DBNull.Value);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            using (var conn = _dbConnection.GetConnection())
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("DELETE FROM employee WHERE employee_id = @id", conn))
                {
                    cmd.Parameters.AddWithValue("id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Employee> GetFiltered(string searchText)
        {
            var employees = new List<Employee>();
            using (var conn = _dbConnection.GetConnection())
            {
                conn.Open();
                string query = @"
                    SELECT employee_id, photo, full_name, position, phone
                    FROM employee
                    WHERE employee_id::text ILIKE @search
                    OR full_name ILIKE @search
                    OR position ILIKE @search
                    OR phone ILIKE @search";
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("search", $"%{searchText}%");
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            employees.Add(new Employee
                            {
                                EmployeeId = reader.GetInt32(0),
                                Photo = reader.IsDBNull(1) ? null : (byte[])reader.GetValue(1),
                                FullName = reader.GetString(2),
                                Position = reader.GetString(3),
                                Phone = reader.IsDBNull(4) ? null : reader.GetString(4)
                            });
                        }
                    }
                }
            }
            return employees;
        }
    }
}