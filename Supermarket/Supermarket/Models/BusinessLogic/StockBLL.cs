using System;
using System.Collections.Generic;
using Supermarket.Models.DataAccessLayer;
using Supermarket.Models.EntityLayer;

namespace Supermarket.Models.BusinessLogic
{
    public class StockBLL
    {
        private StockDAL stockDAL = new StockDAL();

        public List<Stock> GetAllStocks()
        {
            return stockDAL.GetAllStocks();
        }

        public void AddStock(Stock stock)
        {
            stockDAL.AddStock(stock);
        }

        public void EditStock(Stock stock)
        {
            stockDAL.EditStock(stock);
        }

        public void DeleteStock(int stockID)
        {
            stockDAL.DeleteStock(stockID);
        }
        public List<Stock> GetStocksByProductId(int productId) 
        {
            return stockDAL.GetStocksByProductId(productId);
        }

        public void UpdateStockStatus()
        {
            var stocks = stockDAL.GetAllStocks();
            foreach (var stock in stocks)
            {
                if (stock.ExpirationDate <= DateTime.Now)
                {
                    stock.IsActive = false;
                    stockDAL.EditStock(stock);
                }
            }
        }

    }
}
