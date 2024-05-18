using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Supermarket.Models.EntityLayer;

namespace Supermarket.Models.DataAccessLayer
{
    public class ProductDAL
    {
        public List<Product> GetAllProducts()
        {
            List<Product> products = new List<Product>();

            using (SqlConnection con = DALHelper.Connection)
            {
                SqlCommand cmd = new SqlCommand("SELECT p.*, m.ManufacturerName, c.CategoryName FROM Products p LEFT JOIN Manufacturers m ON p.ManufacturerID = m.ManufacturerID LEFT JOIN Categories c ON p.CategoryID = c.CategoryID WHERE p.IsActive = 1", con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Product product = new Product
                    {
                        ProductID = (int)reader["ProductID"],
                        ProductName = reader["ProductName"].ToString(),
                        Barcode = reader["Barcode"].ToString(),
                        CategoryID = reader["CategoryID"] != DBNull.Value ? (int?)reader["CategoryID"] : null,
                        ManufacturerID = reader["ManufacturerID"] != DBNull.Value ? (int?)reader["ManufacturerID"] : null,
                        ManufacturerName = reader["ManufacturerName"] != DBNull.Value ? reader["ManufacturerName"].ToString() : null,
                        CategoryName = reader["CategoryName"] != DBNull.Value ? reader["CategoryName"].ToString() : null,
                        IsActive = (bool)reader["IsActive"]
                    };
                    products.Add(product);
                }
            }

            return products;
        }

        public Product GetProductById(int productId)
        {
            Product product = null;

            using (SqlConnection con = DALHelper.Connection)
            {
                SqlCommand cmd = new SqlCommand("SELECT p.*, m.ManufacturerName, c.CategoryName FROM Products p LEFT JOIN Manufacturers m ON p.ManufacturerID = m.ManufacturerID LEFT JOIN Categories c ON p.CategoryID = c.CategoryID WHERE p.ProductID = @ProductID", con);
                cmd.Parameters.AddWithValue("@ProductID", productId);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    product = new Product
                    {
                        ProductID = (int)reader["ProductID"],
                        ProductName = reader["ProductName"].ToString(),
                        Barcode = reader["Barcode"].ToString(),
                        CategoryID = reader["CategoryID"] != DBNull.Value ? (int?)reader["CategoryID"] : null,
                        ManufacturerID = reader["ManufacturerID"] != DBNull.Value ? (int?)reader["ManufacturerID"] : null,
                        ManufacturerName = reader["ManufacturerName"] != DBNull.Value ? reader["ManufacturerName"].ToString() : null,
                        CategoryName = reader["CategoryName"] != DBNull.Value ? reader["CategoryName"].ToString() : null,
                        IsActive = (bool)reader["IsActive"]
                    };
                }
            }

            return product;
        }

        public void AddProduct(Product product, SqlConnection con, SqlTransaction transaction)
        {
            SqlCommand cmd = new SqlCommand("AddProduct", con, transaction)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@ProductName", product.ProductName);
            cmd.Parameters.AddWithValue("@Barcode", product.Barcode);
            cmd.Parameters.AddWithValue("@CategoryID", (object)product.CategoryID ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@ManufacturerID", (object)product.ManufacturerID ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@IsActive", 1);

            cmd.ExecuteNonQuery();
        }

        public List<Product> SearchProductsInStock()
        {
            List<Product> products = new List<Product>();

            using (SqlConnection con = DALHelper.Connection)
            {
                SqlCommand cmd = new SqlCommand(@"
                    SELECT p.ProductID, p.ProductName, p.Barcode, p.CategoryID, p.ManufacturerID, p.IsActive
                    FROM Products p
                    JOIN ProductStocks ps ON p.ProductID = ps.ProductID
                    WHERE ps.Quantity > 0 AND ps.IsActive = 1", con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Product product = new Product
                    {
                        ProductID = (int)reader["ProductID"],
                        ProductName = reader["ProductName"].ToString(),
                        Barcode = reader["Barcode"].ToString(),
                        CategoryID = reader["CategoryID"] != DBNull.Value ? (int)reader["CategoryID"] : (int?)null,
                        ManufacturerID = reader["ManufacturerID"] != DBNull.Value ? (int)reader["ManufacturerID"] : (int?)null,
                        IsActive = (bool)reader["IsActive"]
                    };
                    products.Add(product);
                }
            }

            return products;
        }

        public void EditProduct(Product product)
        {
            using (SqlConnection con = DALHelper.Connection)
            {
                SqlCommand cmd = new SqlCommand("EditProduct", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@ProductID", product.ProductID);
                cmd.Parameters.AddWithValue("@ProductName", product.ProductName);
                cmd.Parameters.AddWithValue("@Barcode", product.Barcode);
                cmd.Parameters.AddWithValue("@CategoryID", (object)product.CategoryID ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ManufacturerID", (object)product.ManufacturerID ?? DBNull.Value);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteProduct(int productID)
        {
            using (SqlConnection con = DALHelper.Connection)
            {
                SqlCommand cmd = new SqlCommand("DeleteProduct", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@ProductID", productID);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public List<Product> SearchProducts(string productName, string barcode, DateTime? expirationDate, int? manufacturerId, int? categoryId)
        {
            List<Product> products = new List<Product>();

            using (SqlConnection con = DALHelper.Connection)
            {
                SqlCommand cmd = new SqlCommand("SearchProducts", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@ProductName", (object)productName ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Barcode", (object)barcode ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ExpirationDate", (object)expirationDate ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ManufacturerID", (object)manufacturerId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@CategoryID", (object)categoryId ?? DBNull.Value);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Product product = new Product
                    {
                        ProductID = (int)reader["ProductID"],
                        ProductName = reader["ProductName"].ToString(),
                        Barcode = reader["Barcode"].ToString(),
                        ManufacturerID = reader["ManufacturerID"] != DBNull.Value ? (int?)reader["ManufacturerID"] : null,
                        CategoryID = reader["CategoryID"] != DBNull.Value ? (int?)reader["CategoryID"] : null,
                        ManufacturerName = reader["ManufacturerName"] != DBNull.Value ? reader["ManufacturerName"].ToString() : null,
                        CategoryName = reader["CategoryName"] != DBNull.Value ? reader["CategoryName"].ToString() : null,
                        IsActive = (bool)reader["IsActive"]
                    };
                    products.Add(product);
                }
            }

            return products;
        }
    }
}
