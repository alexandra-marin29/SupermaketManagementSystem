using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Supermarket.Models.EntityLayer;

namespace Supermarket.Models.DataAccessLayer
{
    public class ReceiptDAL
    {
        public void AddReceipt(Receipt receipt, List<ReceiptDetail> receiptDetails)
        {
            using (SqlConnection con = DALHelper.Connection)
            {
                SqlCommand cmd = new SqlCommand("AddReceipt", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@CashierID", receipt.CashierID);
                cmd.Parameters.AddWithValue("@ReceiptDate", receipt.ReceiptDate);
                cmd.Parameters.AddWithValue("@AmountCollected", receipt.AmountCollected);

                con.Open();
                int receiptId = Convert.ToInt32(cmd.ExecuteScalar());

                foreach (var detail in receiptDetails)
                {
                    SqlCommand detailCmd = new SqlCommand("AddReceiptDetail", con)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    detailCmd.Parameters.AddWithValue("@ReceiptID", receiptId);
                    detailCmd.Parameters.AddWithValue("@ProductID", detail.ProductID);
                    detailCmd.Parameters.AddWithValue("@Quantity", detail.Quantity);
                    detailCmd.Parameters.AddWithValue("@Subtotal", detail.Subtotal);
                    detailCmd.ExecuteNonQuery();
                }
            }
        }

        public List<Receipt> GetAllReceipts()
        {
            List<Receipt> receipts = new List<Receipt>();

            using (SqlConnection con = DALHelper.Connection)
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Receipts", con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Receipt receipt = new Receipt
                    {
                        ReceiptID = (int)reader["ReceiptID"],
                        CashierID = (int)reader["CashierID"],
                        ReceiptDate = (DateTime)reader["ReceiptDate"],
                        AmountCollected = (decimal)reader["AmountCollected"]
                    };
                    receipts.Add(receipt);
                }
            }

            return receipts;
        }

        public List<ReceiptDetail> GetReceiptDetails(int receiptID)
        {
            List<ReceiptDetail> receiptDetails = new List<ReceiptDetail>();

            using (SqlConnection con = DALHelper.Connection)
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM ReceiptDetails WHERE ReceiptID = @ReceiptID", con);
                cmd.Parameters.AddWithValue("@ReceiptID", receiptID);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    ReceiptDetail detail = new ReceiptDetail
                    {
                        ReceiptDetailID = (int)reader["ReceiptDetailID"],
                        ReceiptID = (int)reader["ReceiptID"],
                        ProductID = (int)reader["ProductID"],
                        Quantity = (decimal)reader["Quantity"],
                        Subtotal = (decimal)reader["Subtotal"]
                    };
                    receiptDetails.Add(detail);
                }
            }

            return receiptDetails;
        }

        public List<Product> SearchProducts(string productName, string barcode, DateTime? expirationDate, int? manufacturerID, int? categoryID)
        {
            List<Product> products = new List<Product>();

            using (SqlConnection con = DALHelper.Connection)
            {
                SqlCommand cmd = new SqlCommand("SearchProducts", con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@ProductName", productName ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Barcode", barcode ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@ExpirationDate", expirationDate ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@ManufacturerID", manufacturerID ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@CategoryID", categoryID ?? (object)DBNull.Value);

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
    }
}
