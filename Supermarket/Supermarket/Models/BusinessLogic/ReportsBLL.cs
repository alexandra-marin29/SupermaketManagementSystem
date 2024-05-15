using System;
using System.Collections.Generic;
using Supermarket.Models.DataAccessLayer;
using Supermarket.ViewModels;

namespace Supermarket.Models.BusinessLogic
{
    public class ReportsBLL
    {
        private ReportsDAL reportsDAL = new ReportsDAL();

        public List<ProductReport> GetProductsByManufacturer(int manufacturerID)
        {
            return reportsDAL.GetProductsByManufacturer(manufacturerID);
        }

        public List<CategoryValueReport> GetCategoryValues(int categoryID)
        {
            return reportsDAL.GetCategoryValues(categoryID);
        }


        public List<SalesReport> GetSalesByUser(int userID, int month, int year)
        {
            return reportsDAL.GetSalesByUser(userID, month, year);
        }

        public List<ReceiptReport> GetLargestReceipt(DateTime date)
        {
            return reportsDAL.GetLargestReceipt(date);
        }
    }
}
