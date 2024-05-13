using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Supermarket.Models.DataAccessLayer;

namespace Supermarket.Models.BusinessLogic
{
    class UserBLL
    {
        UserDAL persoanaDAL = new UserDAL();

        public Role GetUserByLogin(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                return Role.None;
            }
            return persoanaDAL.GetUserByLogin(username, password);
        }
    }
}
