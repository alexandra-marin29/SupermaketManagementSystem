using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Supermarket.Models.EntityLayer;

namespace Supermarket.Models.DataAccessLayer
{
    public class StockDAL
    {
        public List<Stock> GetAllStocks()
        {
            List<Stock> stocks = new List<Stock>();

            using (SqlConnection con = DALHelper.Connection)
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM ProductStocks WHERE IsActive = 1", con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Stock stock = new Stock
                    {
                        StockID = (int)reader["StockID"],
                        ProductID = (int)reader["ProductID"],
                        Quantity = (decimal)reader["Quantity"],
                        UnitOfMeasure = reader["UnitOfMeasure"].ToString(),
                        SupplyDate = (DateTime)reader["SupplyDate"],
                        ExpirationDate = (DateTime)reader["ExpirationDate"],
                        PurchasePrice = (decimal)reader["PurchasePrice"],
                        SalePrice = (decimal)reader["SalePrice"],
                        IsActive = (bool)reader["IsActive"]
                    };
                    stocks.Add(stock);
                }
            }

            return stocks;
        }

        public List<Stock> GetStocksByProductId(int productId) 
        {
            List<Stock> stocks = new List<Stock>();

            using (SqlConnection con = DALHelper.Connection)
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM ProductStocks WHERE ProductID = @ProductID AND IsActive = 1", con);
                cmd.Parameters.AddWithValue("@ProductID", productId);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Stock stock = new Stock
                    {
                        StockID = (int)reader["StockID"],
                        ProductID = (int)reader["ProductID"],
                        Quantity = (decimal)reader["Quantity"],
                        UnitOfMeasure = reader["UnitOfMeasure"].ToString(),
                        SupplyDate = (DateTime)reader["SupplyDate"],
                        ExpirationDate = (DateTime)reader["ExpirationDate"],
                        PurchasePrice = (decimal)reader["PurchasePrice"],
                        SalePrice = (decimal)reader["SalePrice"],
                        IsActive = (bool)reader["IsActive"]
                    };
                    stocks.Add(stock);
                }
            }

            return stocks;
        }

        public void AddStock(Stock stock)
        {
            using (SqlConnection con = DALHelper.Connection)
            {
                SqlCommand cmd = new SqlCommand("AddStock", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@ProductID", stock.ProductID);
                cmd.Parameters.AddWithValue("@Quantity", stock.Quantity);
                cmd.Parameters.AddWithValue("@UnitOfMeasure", stock.UnitOfMeasure);
                cmd.Parameters.AddWithValue("@SupplyDate", stock.SupplyDate);
                cmd.Parameters.AddWithValue("@ExpirationDate", stock.ExpirationDate);
                cmd.Parameters.AddWithValue("@PurchasePrice", stock.PurchasePrice);
                cmd.Parameters.AddWithValue("@SalePrice", stock.SalePrice);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void EditStock(Stock stock)
        {
            using (SqlConnection con = DALHelper.Connection)
            {
                SqlCommand cmd = new SqlCommand("EditStock", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@StockID", stock.StockID);
                cmd.Parameters.AddWithValue("@ProductID", stock.ProductID); 
                cmd.Parameters.AddWithValue("@Quantity", stock.Quantity);
                cmd.Parameters.AddWithValue("@UnitOfMeasure", stock.UnitOfMeasure);
                cmd.Parameters.AddWithValue("@SupplyDate", stock.SupplyDate);
                cmd.Parameters.AddWithValue("@ExpirationDate", stock.ExpirationDate);
                cmd.Parameters.AddWithValue("@PurchasePrice", stock.PurchasePrice);
                cmd.Parameters.AddWithValue("@SalePrice", stock.SalePrice);
                cmd.Parameters.AddWithValue("@IsActive", (object)stock.IsActive ?? DBNull.Value);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }


        public void DeleteStock(int stockID)
        {
            using (SqlConnection con = DALHelper.Connection)
            {
                SqlCommand cmd = new SqlCommand("DeleteStock", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@StockID", stockID);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
