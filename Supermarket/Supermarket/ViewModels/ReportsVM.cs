using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Supermarket.Models.BusinessLogic;
using Supermarket.Models.EntityLayer;
using Supermarket.ViewModels.Commands;

namespace Supermarket.ViewModels
{
    public class ReportsVM : BasePropertyChanged
    {
        private ManufacturerBLL manufacturerBLL = new ManufacturerBLL();
        private CategoryBLL categoryBLL = new CategoryBLL();
        private UserBLL userBLL = new UserBLL();
        private ReportsBLL reportsBLL = new ReportsBLL();

        public ObservableCollection<Manufacturer> Manufacturers { get; set; }
        public ObservableCollection<Category> Categories { get; set; }
        public ObservableCollection<User> Cashiers { get; set; }
        public ObservableCollection<ProductReport> ProductsByManufacturer { get; set; }
        public ObservableCollection<CategoryValueReport> CategoryValues { get; set; }
        public ObservableCollection<SalesReport> SalesByUser { get; set; }
        public ObservableCollection<ReceiptReport> LargestReceipt { get; set; }

        private Manufacturer selectedManufacturer;
        private Category selectedCategory;
        private User selectedCashier;
        private DateTime selectedDate;
        private DateTime selectedMonth;

        public Manufacturer SelectedManufacturer
        {
            get { return selectedManufacturer; }
            set
            {
                selectedManufacturer = value;
                NotifyPropertyChanged(nameof(SelectedManufacturer));
            }
        }

        public Category SelectedCategory
        {
            get { return selectedCategory; }
            set
            {
                selectedCategory = value;
                NotifyPropertyChanged(nameof(SelectedCategory));
            }
        }

        public User SelectedCashier
        {
            get { return selectedCashier; }
            set
            {
                selectedCashier = value;
                NotifyPropertyChanged(nameof(SelectedCashier));
            }
        }

        public DateTime SelectedDate
        {
            get { return selectedDate; }
            set
            {
                selectedDate = value;
                NotifyPropertyChanged(nameof(SelectedDate));
            }
        }

        public DateTime SelectedMonth
        {
            get { return selectedMonth; }
            set
            {
                selectedMonth = value;
                NotifyPropertyChanged(nameof(SelectedMonth));
            }
        }

        public ICommand ListProductsByManufacturerCommand { get; }
        public ICommand ShowCategoryValuesCommand { get; }
        public ICommand ShowSalesByUserCommand { get; }
        public ICommand ShowLargestReceiptCommand { get; }

        public ReportsVM()
        {
            Manufacturers = new ObservableCollection<Manufacturer>(manufacturerBLL.GetAllManufacturers());
            Categories = new ObservableCollection<Category>(categoryBLL.GetAllCategories());
            Cashiers = new ObservableCollection<User>(userBLL.GetAllCashiers());

            ListProductsByManufacturerCommand = new RelayCommand<object>(ListProductsByManufacturer);
            ShowCategoryValuesCommand = new RelayCommand<object>(ShowCategoryValues);
            ShowSalesByUserCommand = new RelayCommand<object>(ShowSalesByUser);
            ShowLargestReceiptCommand = new RelayCommand<object>(ShowLargestReceipt);
        }

        private void ListProductsByManufacturer(object parameter)
        {
            if (SelectedManufacturer != null)
            {
                ProductsByManufacturer = new ObservableCollection<ProductReport>(reportsBLL.GetProductsByManufacturer(SelectedManufacturer.ManufacturerID));
                NotifyPropertyChanged(nameof(ProductsByManufacturer));
            }
        }

        private void ShowCategoryValues(object parameter)
        {
            CategoryValues = new ObservableCollection<CategoryValueReport>(reportsBLL.GetCategoryValues());
            NotifyPropertyChanged(nameof(CategoryValues));
        }

        private void ShowSalesByUser(object parameter)
        {
            if (SelectedCashier != null)
            {
                int month = SelectedMonth.Month;
                int year = SelectedMonth.Year;
                SalesByUser = new ObservableCollection<SalesReport>(reportsBLL.GetSalesByUser(SelectedCashier.UserId, month, year));
                NotifyPropertyChanged(nameof(SalesByUser));
            }
        }

        private void ShowLargestReceipt(object parameter)
        {
            LargestReceipt = new ObservableCollection<ReceiptReport>(reportsBLL.GetLargestReceipt(SelectedDate));
            NotifyPropertyChanged(nameof(LargestReceipt));
        }
    }

    public class ProductReport
    {
        public string ProductName { get; set; }
        public string CategoryName { get; set; }
    }

    public class CategoryValueReport
    {
        public string CategoryName { get; set; }
        public decimal TotalValue { get; set; }
    }

    public class SalesReport
    {
        public DateTime SaleDate { get; set; }
        public decimal DailyTotal { get; set; }
    }

    public class ReceiptReport
    {
        public int SaleID { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
