using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Supermarket.Models;

namespace Supermarket.Models.EntityLayer
{
    public class User : BasePropertyChanged
    {
        private int userId;
        private string username;
        private string password;
        private string role;
        private bool isActive;

        public int UserId
        {
            get { return userId; }
            set
            {
                userId = value;
                NotifyPropertyChanged(nameof(UserId));
            }
        }

        public string Username
        {
            get { return username; }
            set
            {
                username = value;
                NotifyPropertyChanged(nameof(Username));
            }
        }

        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                NotifyPropertyChanged(nameof(Password));
            }
        }

        public string Role
        {
            get { return role; }
            set
            {
                role = value;
                NotifyPropertyChanged(nameof(Role));
            }
        }

        public bool IsActive
        {
            get { return isActive; }
            set
            {
                isActive = value;
                NotifyPropertyChanged(nameof(IsActive));
            }
        }
    }
}