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
                SqlCommand cmd = new SqlCommand("SELECT * FROM Products WHERE IsActive = 1", con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Product product = new Product
                    {
                        ProductID = (int)reader["ProductID"],
                        ProductName = reader["ProductName"].ToString(),
                        Barcode = reader["Barcode"].ToString(),
                        CategoryID = (int)reader["CategoryID"],
                        ManufacturerID = (int)reader["ManufacturerID"],
                        IsActive = (bool)reader["IsActive"]
                    };
                    products.Add(product);
                }
            }

            return products;
        }

        public Product GetProductById(int productId) // Add this method
        {
            Product product = null;

            using (SqlConnection con = DALHelper.Connection)
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Products WHERE ProductID = @ProductID", con);
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
                        CategoryID = (int)reader["CategoryID"],
                        ManufacturerID = (int)reader["ManufacturerID"],
                        IsActive = (bool)reader["IsActive"]
                    };
                }
            }

            return product;
        }

        public void AddProduct(Product product)
        {
            using (SqlConnection con = DALHelper.Connection)
            {
                SqlCommand cmd = new SqlCommand("AddProduct", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@ProductName", product.ProductName);
                cmd.Parameters.AddWithValue("@Barcode", product.Barcode);
                cmd.Parameters.AddWithValue("@CategoryID", product.CategoryID);
                cmd.Parameters.AddWithValue("@ManufacturerID", product.ManufacturerID);
                cmd.Parameters.AddWithValue("@IsActive", 1); // Set IsActive to 1 by default

                con.Open();
                cmd.ExecuteNonQuery();
            }
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
                cmd.Parameters.AddWithValue("@CategoryID", product.CategoryID);
                cmd.Parameters.AddWithValue("@ManufacturerID", product.ManufacturerID);

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
                        ManufacturerID = reader["ManufacturerID"] != DBNull.Value ? (int)reader["ManufacturerID"] : 0,
                        CategoryID = reader["CategoryID"] != DBNull.Value ? (int)reader["CategoryID"] : 0,
                        IsActive = (bool)reader["IsActive"]
                    };
                    products.Add(product);
                }
            }

            return products;
        }


    }
}
