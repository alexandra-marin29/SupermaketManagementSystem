using Supermarket.ViewModels.Commands;
using Supermarket.Views;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using Supermarket.Models;
using Supermarket.Models.EntityLayer;
using Supermarket.Models.BusinessLogic;
using Supermarket.ViewModels;
namespace Supermarket.ViewModels
{
    public class UserVM : BasePropertyChanged
    {
        private readonly Window loginWindow;
        UserBLL userBLL = new UserBLL();

        public UserVM() { }

        public UserVM(Window loginWindow)
        {
            this.loginWindow = loginWindow;
            LoginCommand = new RelayCommand<object>(Login);
        }

        private string username;
        public string Username
        {
            get { return username; }
            set
            {
                if (username != value)
                {
                    username = value;
                    NotifyPropertyChanged(nameof(Username));
                }
            }
        }

        private string password;
        public string Password
        {
            get { return password; }
            set
            {
                if (password != value)
                {
                    password = value;
                    NotifyPropertyChanged(nameof(Password));
                }
            }
        }

        public ICommand LoginCommand { get; }

        #region Commands
        private void Login(object parameter)
        {
            Role role = userBLL.GetUserByLogin(username, password);

            if (role == Role.Admin)
            {
                AdminWindow adminWindow = new AdminWindow();
                adminWindow.Show();
                CloseAllWindows();
            }
            else if (role == Role.Cashier)
            {
                CashierWindow cashierWindow = new CashierWindow();
                cashierWindow.Show();
                CloseAllWindows();
            }
            else
            {
                MessageBox.Show("Invalid username or password.");
            }
        }

        private void CloseAllWindows()
        {
            Application.Current.MainWindow?.Close();
            loginWindow?.Close();
        }
        #endregion
    }
}