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
            if (!categoryDAL.IsCategoryNameExists(category.CategoryName))
            {
                categoryDAL.AddCategory(category);
            }
            else
            {
                throw new Exception("Category name already exists.");
            }
        }

        public void EditCategory(Category category)
        {
            categoryDAL.EditCategory(category);
        }

        public void DeleteCategory(int categoryId)
        {
            if (!categoryDAL.HasActiveProducts(categoryId))
            {
                categoryDAL.DeleteCategory(categoryId);
            }
            else
            {
                throw new Exception("Cannot delete category with existing active products.");
            }
        }
    }
}
