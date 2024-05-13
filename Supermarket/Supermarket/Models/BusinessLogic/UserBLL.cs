using System.Collections.Generic;
using Supermarket.Models.DataAccessLayer;
using Supermarket.Models.EntityLayer;

namespace Supermarket.Models.BusinessLogic
{
    class UserBLL
    {
        UserDAL userDAL = new UserDAL();

        public List<User> GetAllUsers()
        {
            return userDAL.GetAllUsers();
        }

        public void AddUser(User user)
        {
            userDAL.AddUser(user);
        }

        public void EditUser(User user)
        {
            userDAL.EditUser(user);
        }

        public void DeleteUser(int userId)
        {
            userDAL.DeleteUser(userId);
        }

        public Role GetUserByLogin(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                return Role.None;
            }
            return userDAL.GetUserByLogin(username, password);
        }
    }
}
