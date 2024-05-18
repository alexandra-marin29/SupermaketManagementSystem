using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Supermarket.Models.DataAccessLayer;
using Supermarket.Models.EntityLayer;

namespace Supermarket.Models.BusinessLogic
{
    public class ProductBLL
    {
        private ProductDAL productDAL = new ProductDAL();

        public List<Product> GetAllProducts()
        {
            return productDAL.GetAllProducts();
        }

        public Product GetProductById(int productId)
        {
            return productDAL.GetProductById(productId);
        }

        public void AddProduct(Product product)
        {
            using (SqlConnection con = DALHelper.Connection)
            {
                con.Open();
                using (SqlTransaction transaction = con.BeginTransaction())
                {
                    try
                    {
                        if (string.IsNullOrWhiteSpace(product.CategoryName))
                        {
                            throw new ArgumentException("Category name must not be null or empty", nameof(product.CategoryName));
                        }

                        int categoryId = EnsureCategoryExists(product.CategoryName, con, transaction);
                        product.CategoryID = categoryId;

                        if (product.ManufacturerID == null)
                        {
                            throw new ArgumentException("Manufacturer must be selected", nameof(product.ManufacturerID));
                        }

                        productDAL.AddProduct(product, con, transaction);

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        private int EnsureCategoryExists(string categoryName, SqlConnection con, SqlTransaction transaction)
        {
            SqlCommand checkCmd = new SqlCommand("SELECT CategoryID FROM Categories WHERE CategoryName = @CategoryName AND IsActive = 1", con, transaction);
            checkCmd.Parameters.AddWithValue("@CategoryName", categoryName);

            object result = checkCmd.ExecuteScalar();

            if (result != null)
            {
                return (int)result;
            }
            else
            {
                SqlCommand insertCmd = new SqlCommand("INSERT INTO Categories (CategoryName, IsActive) OUTPUT INSERTED.CategoryID VALUES (@CategoryName, 1)", con, transaction);
                insertCmd.Parameters.AddWithValue("@CategoryName", categoryName);

                int newCategoryId = (int)insertCmd.ExecuteScalar();
                return newCategoryId;
            }
        }

        public void EditProduct(Product product)
        {
            productDAL.EditProduct(product);
        }

        public void DeleteProduct(int productID)
        {
            productDAL.DeleteProduct(productID);
        }

        public List<Product> SearchProducts(string productName, string barcode, DateTime? expirationDate, int? manufacturerId, int? categoryId)
        {
            return productDAL.SearchProducts(productName, barcode, expirationDate, manufacturerId, categoryId);
        }

        public List<Product> GetProductsInStock()
        {
            return productDAL.SearchProductsInStock();
        }
    }
}
