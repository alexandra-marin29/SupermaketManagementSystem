using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Supermarket.Models.BusinessLogic;
using Supermarket.Models.EntityLayer;
using Supermarket.ViewModels.Commands;

namespace Supermarket.ViewModels
{
    public class CashierVM : BasePropertyChanged
    {
        private ProductBLL productBLL = new ProductBLL();
        private StockBLL stockBLL = new StockBLL();
        private ManufacturerBLL manufacturerBLL = new ManufacturerBLL();
        private CategoryBLL categoryBLL = new CategoryBLL();

        private string productSearchName;
        private string productSearchBarcode;
        private DateTime? productSearchExpirationDate;
        private Manufacturer selectedManufacturer;
        private Category selectedCategory;
        private Product selectedProduct;

        private ObservableCollection<Product> searchedProducts;
        private ObservableCollection<ReceiptItem> receiptItems;

        public ObservableCollection<Manufacturer> Manufacturers { get; set; }
        public ObservableCollection<Category> Categories { get; set; }

        public string ProductSearchName
        {
            get { return productSearchName; }
            set
            {
                productSearchName = value;
                NotifyPropertyChanged(nameof(ProductSearchName));
            }
        }

        public string ProductSearchBarcode
        {
            get { return productSearchBarcode; }
            set
            {
                productSearchBarcode = value;
                NotifyPropertyChanged(nameof(ProductSearchBarcode));
            }
        }

        public DateTime? ProductSearchExpirationDate
        {
            get { return productSearchExpirationDate; }
            set
            {
                productSearchExpirationDate = value;
                NotifyPropertyChanged(nameof(ProductSearchExpirationDate));
            }
        }

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

        public Product SelectedProduct
        {
            get { return selectedProduct; }
            set
            {
                selectedProduct = value;
                NotifyPropertyChanged(nameof(SelectedProduct));
            }
        }

        public ObservableCollection<Product> SearchedProducts
        {
            get { return searchedProducts; }
            set
            {
                searchedProducts = value;
                NotifyPropertyChanged(nameof(SearchedProducts));
            }
        }

        public ObservableCollection<ReceiptItem> ReceiptItems
        {
            get { return receiptItems; }
            set
            {
                receiptItems = value;
                NotifyPropertyChanged(nameof(ReceiptItems));
                NotifyPropertyChanged(nameof(ReceiptTotal));
            }
        }

        public decimal ReceiptTotal
        {
            get { return ReceiptItems?.Sum(item => item.Subtotal) ?? 0; }
        }

        public ICommand SearchProductsCommand { get; }
        public ICommand AddToReceiptCommand { get; }
        public ICommand FinalizeReceiptCommand { get; }

        public CashierVM()
        {
            Manufacturers = new ObservableCollection<Manufacturer>(manufacturerBLL.GetAllManufacturers());
            Categories = new ObservableCollection<Category>(categoryBLL.GetAllCategories());
            SearchedProducts = new ObservableCollection<Product>();
            ReceiptItems = new ObservableCollection<ReceiptItem>();

            SearchProductsCommand = new RelayCommand<object>(SearchProducts);
            AddToReceiptCommand = new RelayCommand<object>(AddToReceipt);
            FinalizeReceiptCommand = new RelayCommand<object>(FinalizeReceipt);
        }

        private void SearchProducts(object parameter)
        {
            SearchedProducts = new ObservableCollection<Product>(productBLL.SearchProducts(ProductSearchName, ProductSearchBarcode, ProductSearchExpirationDate, SelectedManufacturer?.ManufacturerID, SelectedCategory?.CategoryID));
        }

        private void AddToReceipt(object parameter)
        {
            if (SelectedProduct != null)
            {
                var existingItem = ReceiptItems.FirstOrDefault(item => item.ProductID == SelectedProduct.ProductID);
                if (existingItem != null)
                {
                    existingItem.Quantity++;
                    existingItem.Subtotal = existingItem.Quantity * existingItem.Price;
                }
                else
                {
                    var receiptItem = new ReceiptItem
                    {
                        ProductID = SelectedProduct.ProductID,
                        ProductName = SelectedProduct.ProductName,
                        Quantity = 1,
                        Price = SelectedProduct.SalePrice,
                        Subtotal = SelectedProduct.SalePrice
                    };
                    ReceiptItems.Add(receiptItem);
                }
                NotifyPropertyChanged(nameof(ReceiptTotal));
            }
        }

        private void FinalizeReceipt(object parameter)
        {
            // Update stock quantities
            foreach (var item in ReceiptItems)
            {
                var stock = stockBLL.GetStocksByProductId(item.ProductID)
                                    .OrderBy(s => s.SupplyDate)
                                    .FirstOrDefault(s => s.Quantity >= item.Quantity);
                if (stock != null)
                {
                    stock.Quantity -= item.Quantity;
                    stockBLL.EditStock(stock);
                    if (stock.Quantity == 0)
                    {
                        stock.IsActive = false;
                        stockBLL.EditStock(stock);
                    }
                }
                else
                {
                    MessageBox.Show($"Insufficient stock for {item.ProductName}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            // Save receipt to database (implementation not shown here)

            // Clear receipt
            ReceiptItems.Clear();
            NotifyPropertyChanged(nameof(ReceiptTotal));
            MessageBox.Show("Receipt finalized successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }

    public class ReceiptItem : BasePropertyChanged
    {
        private int productID;
        private string productName;
        private int quantity;
        private decimal price;
        private decimal subtotal;

        public int ProductID
        {
            get { return productID; }
            set
            {
                productID = value;
                NotifyPropertyChanged(nameof(ProductID));
            }
        }

        public string ProductName
        {
            get { return productName; }
            set
            {
                productName = value;
                NotifyPropertyChanged(nameof(ProductName));
            }
        }

        public int Quantity
        {
            get { return quantity; }
            set
            {
                quantity = value;
                NotifyPropertyChanged(nameof(Quantity));
            }
        }

        public decimal Price
        {
            get { return price; }
            set
            {
                price = value;
                NotifyPropertyChanged(nameof(Price));
            }
        }

        public decimal Subtotal
        {
            get { return subtotal; }
            set
            {
                subtotal = value;
                NotifyPropertyChanged(nameof(Subtotal));
            }
        }
    }
}

