using System;
using System.Collections.ObjectModel;
using System.Linq;
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
        private string selectedMonth;
        private DateTime selectedDate;
        private DateTime selectedMonthDateTime;
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

        public string SelectedMonth
        {
            get { return selectedMonth; }
            set
            {
                selectedMonth = value;
                NotifyPropertyChanged(nameof(SelectedMonth));
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

        public DateTime SelectedMonthDateTime
        {
            get { return selectedMonthDateTime; }
            set
            {
                selectedMonthDateTime = value;
                NotifyPropertyChanged(nameof(SelectedMonthDateTime));
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
            SalesByUser = new ObservableCollection<SalesReport>();


            ListProductsByManufacturerCommand = new RelayCommand<object>(ListProductsByManufacturer);
            ShowCategoryValuesCommand = new RelayCommand<object>(ShowCategoryValues);
            ShowSalesByUserCommand = new RelayCommand<object>(ShowSalesByUser);
            ShowLargestReceiptCommand = new RelayCommand<object>(ShowLargestReceipt);

            InitializeReportTypes();
            SelectedDate = new DateTime(2024, 1, 1);
            SelectedMonthDateTime = new DateTime(2024, 1, 1);

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


        public void RefreshReports()
        {
            Manufacturers.Clear();
            foreach (var manufacturer in manufacturerBLL.GetAllManufacturers())
            {
                Manufacturers.Add(manufacturer);
            }

            Categories.Clear();
            foreach (var category in categoryBLL.GetAllCategories())
            {
                Categories.Add(category);
            }

            Cashiers.Clear();
            foreach (var cashier in userBLL.GetAllCashiers())
            {
                Cashiers.Add(cashier);
            }
        }


        private FrameworkElement CreateManufacturerView()
        {
            var stackPanel = new StackPanel();
            stackPanel.Children.Add(new TextBlock { Text = "Select Manufacturer:", Margin = new Thickness(5) });
            var manufacturerComboBox = new ComboBox { Width = 200, ItemsSource = Manufacturers, DisplayMemberPath = "ManufacturerName", Margin = new Thickness(5) };
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
            var categoryComboBox = new ComboBox { Width = 200, ItemsSource = Categories, DisplayMemberPath = "CategoryName", Margin = new Thickness(5) };
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
            var cashierComboBox = new ComboBox
            {
                Width = 150,
                ItemsSource = Cashiers,
                DisplayMemberPath = "Username",
                Margin = new Thickness(5)
            };
            cashierComboBox.SetBinding(ComboBox.SelectedItemProperty, new System.Windows.Data.Binding("SelectedCashier") { Source = this, Mode = System.Windows.Data.BindingMode.TwoWay });
            wrapPanel.Children.Add(cashierComboBox);

            wrapPanel.Children.Add(new TextBlock { Text = "Select Month:", Margin = new Thickness(5) });

            var monthComboBox = new ComboBox
            {
                Width = 150,
                ItemsSource = new string[]
                {
            "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"
                },
                Margin = new Thickness(5)
            };
            monthComboBox.SetBinding(ComboBox.SelectedItemProperty, new System.Windows.Data.Binding("SelectedMonth") { Source = this, Mode = System.Windows.Data.BindingMode.TwoWay });
            wrapPanel.Children.Add(monthComboBox);

            var showButton = new Button
            {
                Content = "Show Sales by User",
                Width = 150,
                Margin = new Thickness(5),
                Command = ShowSalesByUserCommand
            };
            wrapPanel.Children.Add(showButton);

            stackPanel.Children.Add(wrapPanel);

            var salesDataGrid = new DataGrid
            {
                AutoGenerateColumns = false,
                Height = 150,
                IsReadOnly = true,
                ItemsSource = SalesByUser
            };
            salesDataGrid.SetBinding(DataGrid.ItemsSourceProperty, new System.Windows.Data.Binding("SalesByUser") { Source = this });

            salesDataGrid.Columns.Add(new DataGridTextColumn { Header = "Sale Date", Binding = new System.Windows.Data.Binding("SaleDate") { StringFormat = "dd/MM/yyyy" } });
            salesDataGrid.Columns.Add(new DataGridTextColumn { Header = "Daily Total", Binding = new System.Windows.Data.Binding("DailyTotal") });

            stackPanel.Children.Add(new ScrollViewer { Height = 150, Content = salesDataGrid });

            return stackPanel;
        }


        private FrameworkElement CreateReceiptsView()
        {
            var stackPanel = new StackPanel();
            stackPanel.Children.Add(new TextBlock { Text = "Select Date:", Margin = new Thickness(5) });

            var datePicker = new DatePicker
            {
                Width = 200,
                SelectedDate = DateTime.Now,
                Margin = new Thickness(5)
            };
            datePicker.SetBinding(DatePicker.SelectedDateProperty, new System.Windows.Data.Binding("SelectedDate") { Source = this, Mode = System.Windows.Data.BindingMode.TwoWay });

            stackPanel.Children.Add(datePicker);
            stackPanel.Children.Add(new Button { Content = "Show Largest Receipt", Width = 200, Margin = new Thickness(5), Command = ShowLargestReceiptCommand });

            var receiptDataGrid = new DataGrid
            {
                AutoGenerateColumns = false,
                Height = 50, 
                IsReadOnly = true,
                Margin = new Thickness(0, 0, 0, 5)
            };
            receiptDataGrid.SetBinding(DataGrid.ItemsSourceProperty, new System.Windows.Data.Binding("LargestReceipt") { Source = this });

            receiptDataGrid.Columns.Add(new DataGridTextColumn { Header = "Receipt Date", Binding = new System.Windows.Data.Binding("ReceiptDate") { StringFormat = "dd/MM/yyyy" } });
            receiptDataGrid.Columns.Add(new DataGridTextColumn { Header = "Cashier Name", Binding = new System.Windows.Data.Binding("CashierName") });
            receiptDataGrid.Columns.Add(new DataGridTextColumn { Header = "Quantity", Binding = new System.Windows.Data.Binding("Quantity") });
            receiptDataGrid.Columns.Add(new DataGridTextColumn { Header = "Amount Collected", Binding = new System.Windows.Data.Binding("AmountCollected") });

            stackPanel.Children.Add(receiptDataGrid);
            
            var productDetailsDataGrid = new DataGrid
            {
                AutoGenerateColumns = false,
                Height = 150,
                IsReadOnly = true,
                Margin = new Thickness(0, 0, 0, 0)
            };
            productDetailsDataGrid.SetBinding(DataGrid.ItemsSourceProperty, new System.Windows.Data.Binding("SelectedItem.ProductDetails") { Source = receiptDataGrid });

            productDetailsDataGrid.Columns.Add(new DataGridTextColumn { Header = "Product Name", Binding = new System.Windows.Data.Binding("ProductName") });
            productDetailsDataGrid.Columns.Add(new DataGridTextColumn { Header = "Subtotal", Binding = new System.Windows.Data.Binding("Subtotal") });

            var scrollViewer = new ScrollViewer
            {
                Height = 150,
                Content = productDetailsDataGrid
            };

            stackPanel.Children.Add(scrollViewer);

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
            if (SelectedCashier != null && !string.IsNullOrEmpty(SelectedMonth))
            {
                int month = DateTime.ParseExact(SelectedMonth, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                int year = DateTime.Now.Year;

                SalesByUser = new ObservableCollection<SalesReport>(reportsBLL.GetSalesByUser(SelectedCashier.UserId, month, year));

                if (!(SalesByUser.Count > 0))
                {
                    MessageBox.Show("No sales records found.");
                }


                NotifyPropertyChanged(nameof(SalesByUser));
            }
        }

        private void ShowLargestReceipt(object parameter)
        {
            if (SelectedDate != null)
            {
                var receiptReports = reportsBLL.GetLargestReceiptByDate(SelectedDate);
                LargestReceipt = new ObservableCollection<ReceiptReport>(receiptReports);
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
    public class ProductDetail
    {
        public string ProductName { get; set; }
        public decimal Subtotal { get; set; }
    }


    public class ReceiptReport
    {
        public DateTime ReceiptDate { get; set; }
        public string CashierName { get; set; }
        public ObservableCollection<ProductDetail> ProductDetails { get; set; }
        public decimal Quantity { get; set; }
        public decimal AmountCollected { get; set; }
    }


}

