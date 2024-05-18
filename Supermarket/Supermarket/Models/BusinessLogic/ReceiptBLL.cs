using System;
using System.Collections.Generic;
using Supermarket.Models.DataAccessLayer;
using Supermarket.Models.EntityLayer;

namespace Supermarket.Models.BusinessLogic
{
    public class ReceiptBLL
    {
        private ReceiptDAL receiptDAL = new ReceiptDAL();

        public void AddReceipt(Receipt receipt, List<ReceiptDetail> receiptDetails)
        {
            receiptDAL.AddReceipt(receipt, receiptDetails);
        }

        public List<Receipt> GetAllReceipts()
        {
            return receiptDAL.GetAllReceipts();
        }

        public List<ReceiptDetail> GetReceiptDetails(int receiptID)
        {
            return receiptDAL.GetReceiptDetails(receiptID);
        }

        public List<Product> SearchProducts(string productName, string barcode, DateTime? expirationDate, int? manufacturerID, int? categoryID)
        {
            return receiptDAL.SearchProducts(productName, barcode, expirationDate, manufacturerID, categoryID);
        }


    }
}
