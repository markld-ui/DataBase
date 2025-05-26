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
    public class ProductRepository : IProductRepository
    {
        private readonly DatabaseConnection _dbConnection;

        public ProductRepository(DatabaseConnection dbConnection)
        {
            _dbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
        }

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

        public Product GetById(int id)
        {
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

        public void Add(Product product)
        {
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

        public void Update(Product product)
        {
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
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            using (var conn = _dbConnection.GetConnection())
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("DELETE FROM product WHERE product_id = @id", conn))
                {
                    cmd.Parameters.AddWithValue("id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Product> GetFiltered(string searchText)
        {
            var products = new List<Product>();
            using (var conn = _dbConnection.GetConnection())
            {
                conn.Open();
                string query = @"
                    SELECT product_id, name, expiry_date, product_type, is_active, photo
                    FROM product
                    WHERE product_id::text ILIKE @search
                    OR name ILIKE @search
                    OR expiry_date::text ILIKE @search
                    OR product_type::text ILIKE @search
                    OR is_active::text ILIKE @search";
                using (var cmd = new NpgsqlCommand(query, conn))
                {
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