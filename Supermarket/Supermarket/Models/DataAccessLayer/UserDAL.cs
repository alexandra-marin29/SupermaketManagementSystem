using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Supermarket.Models.EntityLayer;

namespace Supermarket.Models.DataAccessLayer
{
    class UserDAL
    {
        public List<User> GetAllUsers()
        {
            List<User> users = new List<User>();

            using (SqlConnection con = DALHelper.Connection)
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Users WHERE IsActive = 1", con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    User user = new User
                    {
                        UserId = (int)reader["UserId"],
                        Username = reader["Username"].ToString(),
                        Password = reader["Password"].ToString(),
                        Role = reader["Role"].ToString(),
                        IsActive = (bool)reader["IsActive"]
                    };
                    users.Add(user);
                }
            }

            return users;
        }

        public List<User> GetAllCashiers()
        {
            List<User> cashiers = new List<User>();

            using (SqlConnection con = DALHelper.Connection)
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Users WHERE Role = 'Cashier' AND IsActive = 1", con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    User user = new User
                    {
                        UserId = (int)reader["UserID"],
                        Username = reader["Username"].ToString(),
                        Role = reader["Role"].ToString(),
                        IsActive = (bool)reader["IsActive"]
                    };
                    cashiers.Add(user);
                }
            }

            return cashiers;
        }

        public void AddUser(User user)
        {
            using (SqlConnection con = DALHelper.Connection)
            {
                SqlCommand cmd = new SqlCommand("AddUser", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@Username", user.Username);
                cmd.Parameters.AddWithValue("@Password", user.Password);
                cmd.Parameters.AddWithValue("@Role", user.Role);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void EditUser(User user)
        {
            using (SqlConnection con = DALHelper.Connection)
            {
                SqlCommand cmd = new SqlCommand("EditUser", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@UserId", user.UserId);
                cmd.Parameters.AddWithValue("@Username", user.Username);
                cmd.Parameters.AddWithValue("@Password", user.Password);
                cmd.Parameters.AddWithValue("@Role", user.Role);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteUser(int userId)
        {
            using (SqlConnection con = DALHelper.Connection)
            {
                SqlCommand cmd = new SqlCommand("DeleteUser", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@UserId", userId);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public User GetUserByLogin(string username, string password)
        {
            using (SqlConnection con = DALHelper.Connection)
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("GetPersonByLogin", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Avoid SQL injection
                    cmd.Parameters.Add(new SqlParameter("@usern", SqlDbType.NVarChar)).Value = username;
                    cmd.Parameters.Add(new SqlParameter("@psw", SqlDbType.NVarChar)).Value = password;

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    User user = null;
                    if (reader.Read())
                    {
                        user = new User
                        {
                            UserId = (int)reader["UserID"],
                            Username = reader["Username"].ToString(),
                            Role = reader["Role"].ToString(),
                            IsActive = (bool)reader["IsActive"]
                        };
                    }
                    reader.Close();

                    return user;
                }
                catch (Exception ex)
                {
                    throw new Exception("An error occurred while getting the user by login.", ex);
                }
                finally
                {
                    con.Close();
                }
            }
        }
        public bool IsUsernameExists(string username)
        {
            using (SqlConnection con = DALHelper.Connection)
            {
                SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Users WHERE Username = @Username AND IsActive = 1", con);
                cmd.Parameters.AddWithValue("@Username", username);

                con.Open();
                int count = (int)cmd.ExecuteScalar();

                return count > 0;
            }
        }
    }
}
