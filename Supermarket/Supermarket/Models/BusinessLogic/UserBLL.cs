using System;
using System.Collections.Generic;
using Supermarket.Models.DataAccessLayer;
using Supermarket.Models.EntityLayer;

namespace Supermarket.Models.BusinessLogic
{
    public class UserBLL
    {
        UserDAL userDAL = new UserDAL();

        public List<User> GetAllUsers()
        {
            return userDAL.GetAllUsers();
        }

        public List<User> GetAllCashiers()
        {
            return userDAL.GetAllCashiers();
        }

        public void AddUser(User user)
        {
            if (!userDAL.IsUsernameExists(user.Username))
            {
                userDAL.AddUser(user);
            }
            else
            {
                throw new Exception("Username already exists.");
            }
        }

        public void EditUser(User user)
        {
            userDAL.EditUser(user);
        }

        public void DeleteUser(int userId)
        {
            userDAL.DeleteUser(userId);
        }

        public User GetUserByLogin(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                return null;
            }
            return userDAL.GetUserByLogin(username, password);
        }
    }
}
