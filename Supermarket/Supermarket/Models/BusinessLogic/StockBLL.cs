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
    }
}
