using Supermarket.ViewModels.Commands;
using Supermarket.Views;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows;
using Supermarket.Models;
using Supermarket.Models.EntityLayer;
using Supermarket.Models.BusinessLogic;
using System.Text.RegularExpressions;

namespace Supermarket.ViewModels
{
    public class UserVM : BasePropertyChanged
    {
        private readonly Window loginWindow;
        private UserBLL userBLL;
        private string username;
        private string password;

        public ObservableCollection<User> Users { get; set; }
        public ObservableCollection<string> AvailableRoles { get; set; }

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

        private string newUsername;
        public string NewUsername
        {
            get { return newUsername; }
            set
            {
                if (newUsername != value)
                {
                    newUsername = value;
                    NotifyPropertyChanged(nameof(NewUsername));
                }
            }
        }

        private string newPassword;
        public string NewPassword
        {
            get { return newPassword; }
            set
            {
                if (newPassword != value)
                {
                    newPassword = value;
                    NotifyPropertyChanged(nameof(NewPassword));
                }
            }
        }

        private string newRole;
        public string NewRole
        {
            get { return newRole; }
            set
            {
                if (newRole != value)
                {
                    newRole = value;
                    NotifyPropertyChanged(nameof(NewRole));
                }
            }
        }

        private User selectedUser;
        public User SelectedUser
        {
            get { return selectedUser; }
            set
            {
                selectedUser = value;
                NotifyPropertyChanged(nameof(SelectedUser));
            }
        }

        public ICommand LoginCommand { get; }
        public ICommand AddUserCommand { get; }
        public ICommand EditUserCommand { get; }
        public ICommand DeleteUserCommand { get; }

        public UserVM()
        {
            userBLL = new UserBLL();
            Users = new ObservableCollection<User>(userBLL.GetAllUsers());
            AvailableRoles = new ObservableCollection<string> { "Admin", "Cashier" };
            AddUserCommand = new RelayCommand<object>(AddUser);
            EditUserCommand = new RelayCommand<object>(EditUser);
            DeleteUserCommand = new RelayCommand<object>(DeleteUser);
        }

        public UserVM(Window loginWindow) : this()
        {
            this.loginWindow = loginWindow;
            LoginCommand = new RelayCommand<object>(Login);
        }

        #region Commands
        private void Login(object parameter)
        {
            User user = userBLL.GetUserByLogin(Username, Password);

            if (user != null)
            {
                SessionManager.CurrentUser = user;

                if (user.Role == "Admin")
                {
                    AdminWindow adminWindow = new AdminWindow();
                    adminWindow.Show();
                    CloseAllWindows();
                }
                else if (user.Role == "Cashier")
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
            else
            {
                MessageBox.Show("Invalid username or password.");
            }
        }


        private void AddUser(object parameter)
        {
            if (ValidateUserInputs(NewUsername, NewPassword, NewRole))
            {
                try
                {
                    User newUser = new User { Username = NewUsername, Password = NewPassword, Role = NewRole, IsActive = true };
                    userBLL.AddUser(newUser);
                    Users.Add(newUser);
                    NewUsername = string.Empty;
                    NewPassword = string.Empty;
                    NewRole = string.Empty;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void EditUser(object parameter)
        {
            if (SelectedUser != null && ValidateUserInputs(SelectedUser.Username, SelectedUser.Password, SelectedUser.Role))
            {
                userBLL.EditUser(SelectedUser);
                int index = Users.IndexOf(SelectedUser);
                Users[index] = SelectedUser;
            }
        }

        private void DeleteUser(object parameter)
        {
            if (SelectedUser != null)
            {
                userBLL.DeleteUser(SelectedUser.UserId);
                Users.Remove(SelectedUser);
            }
        }

        private bool ValidateUserInputs(string username, string password, string role)
        {
            if (string.IsNullOrWhiteSpace(username) || !Regex.IsMatch(username, @"^[a-zA-Z0-9]+$"))
            {
                MessageBox.Show("Username must be non-empty and contain only letters and digits.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(password) || !Regex.IsMatch(password, @"^[a-zA-Z0-9]+$"))
            {
                MessageBox.Show("Password must be non-empty and contain only letters and digits.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(role) || !(role == "Admin" || role == "Cashier"))
            {
                MessageBox.Show("Role must be either 'Admin' or 'Cashier'.");
                return false;
            }

            return true;
        }

        private void CloseAllWindows()
        {
            Application.Current.MainWindow?.Close();
            loginWindow?.Close();
        }
        #endregion
    }
}
