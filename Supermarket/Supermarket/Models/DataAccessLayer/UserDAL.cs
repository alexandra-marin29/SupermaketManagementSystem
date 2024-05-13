using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Supermarket.Models.DataAccessLayer
{
    class UserDAL
    {
        public Role GetUserByLogin(string username, string password)
        {
            using (SqlConnection con = DALHelper.Connection)
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("GetPersonByLogin", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@usern", username));
                    cmd.Parameters.Add(new SqlParameter("@psw", password));

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    string queryResult = null;
                    if (reader.Read())
                    {
                        queryResult = reader["Role"].ToString();
                    }
                    reader.Close();

                    if (queryResult == "Admin")
                    {
                        return Role.Admin;
                    }
                    if (queryResult == "Cashier")
                    {
                        return Role.Cashier;
                    }
                    return Role.None;
                }
                catch (Exception ex)
                {
                    // Handle exceptions (logging, rethrowing, etc.)
                    throw new Exception("An error occurred while getting the user by login.", ex);
                }
                finally
                {
                    con.Close();
                }
            }
        }
    }
}
