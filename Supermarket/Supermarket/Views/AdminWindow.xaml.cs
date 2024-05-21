using System.Windows;
using System.Windows.Controls;

namespace Supermarket.Views
{
    public partial class AdminWindow : Window
    {
        public AdminWindow()
        {
            InitializeComponent();
            this.DataContext = new AdminWindowVM();
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is TabControl)
            {
                var viewModel = DataContext as AdminWindowVM;

                if (CategoriesTab.IsSelected)
                {
                    viewModel.CategoryVM.RefreshCategories();
                }
                else if (ManufacturersTab.IsSelected)
                {
                    viewModel.ManufacturerVM.RefreshManufacturers();
                }
                else if (ProductsTab.IsSelected)
                {
                    viewModel.ProductVM.RefreshProducts();
                }
                else if (StocksTab.IsSelected)
                {
                    viewModel.StockVM.RefreshStocks();
                }
                else if (ReportsTab.IsSelected)
                {
                    viewModel.ReportsVM.RefreshReports();
                }
            }
        }

    }
}
