using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Supermarket.Models.EntityLayer;

namespace Supermarket.Models.DataAccessLayer
{
    public class CategoryDAL
    {
        public List<Category> GetAllCategories()
        {
            List<Category> categories = new List<Category>();

            using (SqlConnection con = DALHelper.Connection)
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Categories WHERE IsActive = 1", con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Category category = new Category
                    {
                        CategoryID = (int)reader["CategoryId"],
                        CategoryName = reader["CategoryName"].ToString(),
                        IsActive = (bool)reader["IsActive"]
                    };
                    categories.Add(category);
                }
            }

            return categories;
        }

        public void AddCategory(Category category)
        {
            using (SqlConnection con = DALHelper.Connection)
            {
                SqlCommand cmd = new SqlCommand("AddCategory", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@CategoryName", category.CategoryName);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void EditCategory(Category category)
        {
            using (SqlConnection con = DALHelper.Connection)
            {
                SqlCommand cmd = new SqlCommand("EditCategory", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@CategoryId", category.CategoryID);
                cmd.Parameters.AddWithValue("@CategoryName", category.CategoryName);

                con.Open();
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        public void DeleteCategory(int categoryId)
        {
            using (SqlConnection con = DALHelper.Connection)
            {
                SqlCommand cmd = new SqlCommand("DeleteCategory", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@CategoryId", categoryId);

                con.Open();
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        public bool HasActiveProducts(int categoryId)
        {
            using (SqlConnection con = DALHelper.Connection)
            {
                SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Products WHERE CategoryID = @CategoryId AND IsActive = 1", con);
                cmd.Parameters.AddWithValue("@CategoryId", categoryId);

                con.Open();
                int count = (int)cmd.ExecuteScalar();

                return count > 0;
            }
        }

        public bool IsCategoryNameExists(string categoryName)
        {
            using (SqlConnection con = DALHelper.Connection)
            {
                SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Categories WHERE CategoryName = @CategoryName AND IsActive = 1", con);
                cmd.Parameters.AddWithValue("@CategoryName", categoryName);

                con.Open();
                int count = (int)cmd.ExecuteScalar();

                return count > 0;
            }
        }

    }
}
