using System;
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
            if (!categoryDAL.HasProducts(category.CategoryID))
            {
                categoryDAL.EditCategory(category);
            }
            else
            {
                throw new Exception("Cannot edit category with existing products.");
            }
        }

        public void DeleteCategory(int categoryId)
        {
            if (!categoryDAL.HasProducts(categoryId))
            {
                categoryDAL.DeleteCategory(categoryId);
            }
            else
            {
                throw new Exception("Cannot delete category with existing products.");
            }
        }
    }
}
