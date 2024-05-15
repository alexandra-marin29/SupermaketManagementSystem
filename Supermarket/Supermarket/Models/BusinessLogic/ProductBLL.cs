using System.Collections.Generic;
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

        public void AddProduct(Product product)
        {
            productDAL.AddProduct(product);
        }

        public void EditProduct(Product product)
        {
            productDAL.EditProduct(product);
        }

        public void DeleteProduct(int productID)
        {
            productDAL.DeleteProduct(productID);
        }
    }
}
