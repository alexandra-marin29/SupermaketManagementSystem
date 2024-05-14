using System.Collections.Generic;
using Supermarket.Models.DataAccessLayer;
using Supermarket.Models.EntityLayer;

namespace Supermarket.Models.BusinessLogic
{
    public class CategoryBLL
    {
        private CategoryDAL categoryDAL = new CategoryDAL();

        public List<Category> GetAllCategories()
        {
            return categoryDAL.GetAllCategories();
        }

        public void AddCategory(Category category)
        {
            categoryDAL.AddCategory(category);
        }

        public void EditCategory(Category category)
        {
            categoryDAL.EditCategory(category);
        }

        public void DeleteCategory(int categoryId)
        {
            categoryDAL.DeleteCategory(categoryId);
        }
    }
}
