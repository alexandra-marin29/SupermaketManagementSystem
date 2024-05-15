using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Supermarket.Models.BusinessLogic;
using Supermarket.Models.EntityLayer;
using Supermarket.ViewModels.Commands;

namespace Supermarket.ViewModels
{
    public class ReportType
    {
        public string Name { get; set; }
        public FrameworkElement View { get; set; }
    }

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

        public ObservableCollection<ReportType> ReportTypes { get; set; }

        private Manufacturer selectedManufacturer;
        private Category selectedCategory;
        private User selectedCashier;
        private DateTime selectedDate;
        private DateTime selectedMonth;
        private ReportType selectedReportType;

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

        public ReportType SelectedReportType
        {
            get { return selectedReportType; }
            set
            {
                selectedReportType = value;
                NotifyPropertyChanged(nameof(SelectedReportType));
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

            InitializeReportTypes();
            SelectedDate = new DateTime(2024, 1, 1);
            SelectedMonth = new DateTime(2024, 1, 1);

        }

        private void InitializeReportTypes()
        {
            ReportTypes = new ObservableCollection<ReportType>
            {
                new ReportType { Name = "Manufacturers", View = CreateManufacturerView() },
                new ReportType { Name = "Categories", View = CreateCategoryView() },
                new ReportType { Name = "Sales", View = CreateSalesView() },
                new ReportType { Name = "Receipts", View = CreateReceiptsView() }
            };
        }

        private FrameworkElement CreateManufacturerView()
        {
            var stackPanel = new StackPanel();
            stackPanel.Children.Add(new TextBlock { Text = "Select Manufacturer:", Margin = new Thickness(5) });
            var manufacturerComboBox = new ComboBox { Width = 200, ItemsSource = Manufacturers, DisplayMemberPath = "ManufacturerName", SelectedItem = SelectedManufacturer, Margin = new Thickness(5) };
            manufacturerComboBox.SetBinding(ComboBox.SelectedItemProperty, new System.Windows.Data.Binding("SelectedManufacturer") { Source = this, Mode = System.Windows.Data.BindingMode.TwoWay });
            stackPanel.Children.Add(manufacturerComboBox);
            var listButton = new Button { Content = "List Products by Manufacturer", Width = 200, Margin = new Thickness(5), Command = ListProductsByManufacturerCommand };
            stackPanel.Children.Add(listButton);
            var productsDataGrid = new DataGrid { AutoGenerateColumns = true, Height = 150, ItemsSource = ProductsByManufacturer };
            productsDataGrid.SetBinding(DataGrid.ItemsSourceProperty, new System.Windows.Data.Binding("ProductsByManufacturer") { Source = this });
            stackPanel.Children.Add(new ScrollViewer { Height = 150, Content = productsDataGrid });
            return stackPanel;
        }

        private FrameworkElement CreateCategoryView()
        {
            var stackPanel = new StackPanel();
            stackPanel.Children.Add(new TextBlock { Text = "Select Category:", Margin = new Thickness(5) });
            var categoryComboBox = new ComboBox { Width = 200, ItemsSource = Categories, DisplayMemberPath = "CategoryName", SelectedItem = SelectedCategory, Margin = new Thickness(5) };
            categoryComboBox.SetBinding(ComboBox.SelectedItemProperty, new System.Windows.Data.Binding("SelectedCategory") { Source = this, Mode = System.Windows.Data.BindingMode.TwoWay });
            stackPanel.Children.Add(categoryComboBox);
            var showButton = new Button { Content = "Show Category Values", Width = 200, Margin = new Thickness(5), Command = ShowCategoryValuesCommand };
            stackPanel.Children.Add(showButton);
            var categoryValuesDataGrid = new DataGrid { AutoGenerateColumns = true, Height = 150, ItemsSource = CategoryValues };
            categoryValuesDataGrid.SetBinding(DataGrid.ItemsSourceProperty, new System.Windows.Data.Binding("CategoryValues") { Source = this });
            stackPanel.Children.Add(new ScrollViewer { Height = 150, Content = categoryValuesDataGrid });
            return stackPanel;
        }

        private FrameworkElement CreateSalesView()
        {
            var stackPanel = new StackPanel();
            stackPanel.Children.Add(new TextBlock { Text = "Select Cashier:", Margin = new Thickness(5) });

            var wrapPanel = new WrapPanel();
            wrapPanel.Children.Add(new ComboBox
            {
                Width = 150,
                ItemsSource = Cashiers,
                DisplayMemberPath = "Username",
                SelectedItem = SelectedCashier,
                Margin = new Thickness(5)
            });

            wrapPanel.Children.Add(new TextBlock { Text = "Select Month:", Margin = new Thickness(5) });

            var datePicker = new DatePicker
            {
                Width = 150,
                SelectedDate = SelectedMonth != default(DateTime) ? SelectedMonth : new DateTime(2024, 1, 1),
                Margin = new Thickness(5)
            };
            wrapPanel.Children.Add(datePicker);

            wrapPanel.Children.Add(new Button
            {
                Content = "Show Sales by User",
                Width = 150,
                Margin = new Thickness(5),
                Command = ShowSalesByUserCommand
            });

            stackPanel.Children.Add(wrapPanel);
            stackPanel.Children.Add(new ScrollViewer { Height = 150, Content = new DataGrid { AutoGenerateColumns = true, ItemsSource = SalesByUser } });
            return stackPanel;
        }


        private FrameworkElement CreateReceiptsView()
        {
            var stackPanel = new StackPanel();
            stackPanel.Children.Add(new TextBlock { Text = "Select Date:", Margin = new Thickness(5) });

            var datePicker = new DatePicker
            {
                Width = 200,
                SelectedDate = new DateTime(2024, 1, 1),
                DisplayDateStart = new DateTime(2024, 1, 1),
                DisplayDateEnd = new DateTime(2024, 12, 31),
                Margin = new Thickness(5)
            };
            datePicker.SelectedDateChanged += (s, e) =>
            {
                if (datePicker.SelectedDate < new DateTime(2024, 1, 1))
                {
                    datePicker.SelectedDate = new DateTime(2024, 1, 1);
                }
                else if (datePicker.SelectedDate > new DateTime(2024, 12, 31))
                {
                    datePicker.SelectedDate = new DateTime(2024, 12, 31);
                }
            };

            stackPanel.Children.Add(datePicker);
            stackPanel.Children.Add(new Button { Content = "Show Largest Receipt", Width = 200, Margin = new Thickness(5), Command = ShowLargestReceiptCommand });
            stackPanel.Children.Add(new ScrollViewer { Height = 150, Content = new DataGrid { AutoGenerateColumns = true, ItemsSource = LargestReceipt } });
            return stackPanel;
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
            if (SelectedCategory != null)
            {
                CategoryValues = new ObservableCollection<CategoryValueReport>(reportsBLL.GetCategoryValues(SelectedCategory.CategoryID));
                NotifyPropertyChanged(nameof(CategoryValues));
            }
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
            if (SelectedDate != null)
            {
                LargestReceipt = new ObservableCollection<ReceiptReport>(reportsBLL.GetLargestReceiptByDate(SelectedDate));
                NotifyPropertyChanged(nameof(LargestReceipt));
            }
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
        public int ReceiptID { get; set; }
        public DateTime ReceiptDate { get; set; }
        public int CashierID { get; set; }
        public decimal AmountCollected { get; set; }
        public int ProductID { get; set; }
        public decimal Quantity { get; set; }
        public decimal Subtotal { get; set; }
    }
}

