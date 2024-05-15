using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Supermarket.Models.EntityLayer;
using Supermarket.ViewModels;

namespace Supermarket.Models.DataAccessLayer
{
    public class ReportsDAL
    {
        public List<ProductReport> GetProductsByManufacturer(int manufacturerID)
        {
            List<ProductReport> products = new List<ProductReport>();

            using (SqlConnection con = DALHelper.Connection)
            {
                SqlCommand cmd = new SqlCommand("GetProductsByManufacturer", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@ManufacturerID", manufacturerID);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    ProductReport product = new ProductReport
                    {
                        ProductName = reader["ProductName"].ToString(),
                        CategoryName = reader["CategoryName"].ToString()
                    };
                    products.Add(product);
                }
            }

            return products;
        }

        public List<CategoryValueReport> GetCategoryValues()
        {
            List<CategoryValueReport> categories = new List<CategoryValueReport>();

            using (SqlConnection con = DALHelper.Connection)
            {
                SqlCommand cmd = new SqlCommand("GetCategoryValues", con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    CategoryValueReport categoryValue = new CategoryValueReport
                    {
                        CategoryName = reader["CategoryName"].ToString(),
                        TotalValue = (decimal)reader["TotalValue"]
                    };
                    categories.Add(categoryValue);
                }
            }

            return categories;
        }

        public List<SalesReport> GetSalesByUser(int userID, int month, int year)
        {
            List<SalesReport> sales = new List<SalesReport>();

            using (SqlConnection con = DALHelper.Connection)
            {
                SqlCommand cmd = new SqlCommand("GetSalesByUser", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@UserID", userID);
                cmd.Parameters.AddWithValue("@Month", month);
                cmd.Parameters.AddWithValue("@Year", year);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    SalesReport sale = new SalesReport
                    {
                        SaleDate = (DateTime)reader["SaleDate"],
                        DailyTotal = (decimal)reader["DailyTotal"]
                    };
                    sales.Add(sale);
                }
            }

            return sales;
        }

        public List<ReceiptReport> GetLargestReceipt(DateTime date)
        {
            List<ReceiptReport> receipts = new List<ReceiptReport>();

            using (SqlConnection con = DALHelper.Connection)
            {
                SqlCommand cmd = new SqlCommand("GetLargestReceipt", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@Date", date);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    ReceiptReport receipt = new ReceiptReport
                    {
                        SaleID = (int)reader["SaleID"],
                        TotalAmount = (decimal)reader["TotalAmount"]
                    };
                    receipts.Add(receipt);
                }
            }

            return receipts;
        }
    }
}


