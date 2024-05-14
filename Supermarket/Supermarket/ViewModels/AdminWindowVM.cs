using Supermarket.ViewModels;
using System.ComponentModel;

namespace Supermarket.Views
{
    public class AdminWindowVM : INotifyPropertyChanged
    {
        public UserVM UserVM { get; set; }
        public CategoryVM CategoryVM { get; set; }

        public AdminWindowVM()
        {
            UserVM = new UserVM();
            CategoryVM = new CategoryVM();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
