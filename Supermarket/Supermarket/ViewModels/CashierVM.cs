using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Supermarket.Models;
using Supermarket.Models.BusinessLogic;
using Supermarket.Models.EntityLayer;
using Supermarket.ViewModels.Commands;

namespace Supermarket.ViewModels
{
    public class CashierVM : BasePropertyChanged
    {
        private ProductBLL productBLL = new ProductBLL();
        private ReceiptBLL receiptBLL = new ReceiptBLL();
        private StockBLL stockBLL = new StockBLL();
        private ManufacturerBLL manufacturerBLL = new ManufacturerBLL();
        private CategoryBLL categoryBLL = new CategoryBLL();

        private ObservableCollection<Product> products;
        private ObservableCollection<ReceiptDetail> receiptDetails;
        private ObservableCollection<string> productNames;
        private ObservableCollection<string> barcodes;
        private ObservableCollection<Manufacturer> manufacturers;
        private ObservableCollection<Category> categories;
        private Product selectedProduct;
        private ReceiptDetail selectedReceiptDetail;
        private decimal quantity;
        private decimal totalAmount;
        private string productName;
        private string barcode;
        private DateTime? expirationDate;
        private Manufacturer selectedManufacturer;
        private Category selectedCategory;

        public CashierVM()
        {
            Products = new ObservableCollection<Product>();
            ReceiptDetails = new ObservableCollection<ReceiptDetail>();
            ProductNames = new ObservableCollection<string>(productBLL.GetAllProducts().Select(p => p.ProductName).Distinct());
            Barcodes = new ObservableCollection<string>(productBLL.GetAllProducts().Select(p => p.Barcode).Distinct());
            Manufacturers = new ObservableCollection<Manufacturer>(manufacturerBLL.GetAllManufacturers());
            Categories = new ObservableCollection<Category>(categoryBLL.GetAllCategories());

            SearchProductsCommand = new RelayCommand<object>(SearchProducts);
            AddToReceiptCommand = new RelayCommand<object>(AddToReceipt);
            FinalizeReceiptCommand = new RelayCommand<object>(FinalizeReceipt);
        }

        public ObservableCollection<Product> Products
        {
            get { return products; }
            set
            {
                products = value;
                NotifyPropertyChanged(nameof(Products));
            }
        }

        public ObservableCollection<ReceiptDetail> ReceiptDetails
        {
            get { return receiptDetails; }
            set
            {
                receiptDetails = value;
                NotifyPropertyChanged(nameof(ReceiptDetails));
            }
        }

        public ObservableCollection<string> ProductNames
        {
            get { return productNames; }
            set
            {
                productNames = value;
                NotifyPropertyChanged(nameof(ProductNames));
            }
        }

        public ObservableCollection<string> Barcodes
        {
            get { return barcodes; }
            set
            {
                barcodes = value;
                NotifyPropertyChanged(nameof(Barcodes));
            }
        }

        public ObservableCollection<Manufacturer> Manufacturers
        {
            get { return manufacturers; }
            set
            {
                manufacturers = value;
                NotifyPropertyChanged(nameof(Manufacturers));
            }
        }

        public ObservableCollection<Category> Categories
        {
            get { return categories; }
            set
            {
                categories = value;
                NotifyPropertyChanged(nameof(Categories));
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

        public ReceiptDetail SelectedReceiptDetail
        {
            get { return selectedReceiptDetail; }
            set
            {
                selectedReceiptDetail = value;
                NotifyPropertyChanged(nameof(SelectedReceiptDetail));
            }
        }

        public decimal Quantity
        {
            get { return quantity; }
            set
            {
                quantity = value;
                NotifyPropertyChanged(nameof(Quantity));
            }
        }

        public decimal TotalAmount
        {
            get { return totalAmount; }
            set
            {
                totalAmount = value;
                NotifyPropertyChanged(nameof(TotalAmount));
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

        public string Barcode
        {
            get { return barcode; }
            set
            {
                barcode = value;
                NotifyPropertyChanged(nameof(Barcode));
            }
        }

        public DateTime? ExpirationDate
        {
            get { return expirationDate; }
            set
            {
                expirationDate = value;
                NotifyPropertyChanged(nameof(ExpirationDate));
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

        public ICommand SearchProductsCommand { get; }
        public ICommand AddToReceiptCommand { get; }
        public ICommand FinalizeReceiptCommand { get; }

        private void SearchProducts(object parameter)
        {
            Products.Clear();
            var productsList = productBLL.SearchProducts(ProductName, Barcode, ExpirationDate, SelectedManufacturer?.ManufacturerID, SelectedCategory?.CategoryID);
            foreach (var product in productsList)
            {
                Products.Add(product);
            }
        }

        private void AddToReceipt(object parameter)
        {
            if (SelectedProduct != null && Quantity > 0)
            {
                var stock = stockBLL.GetStocksByProductId(SelectedProduct.ProductID)
                    .FirstOrDefault(s => s.ExpirationDate > DateTime.Now && s.Quantity >= Quantity);

                if (stock != null)
                {
                    decimal salePrice = stock.SalePrice;
                    decimal subtotal = salePrice * Quantity;

                    var receiptDetail = new ReceiptDetail
                    {
                        ProductID = SelectedProduct.ProductID,
                        ProductName = SelectedProduct.ProductName,
                        Quantity = Quantity,
                        Subtotal = subtotal
                    };

                    ReceiptDetails.Add(receiptDetail);
                    TotalAmount += subtotal;

                    stock.Quantity -= Quantity;
                    if (stock.Quantity == 0)
                    {
                        stock.IsActive = false;
                    }
                    stockBLL.EditStock(stock);

                    ClearFields();
                }
                else
                {
                    MessageBox.Show("Insufficient stock or expired product.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void FinalizeReceipt(object parameter)
        {
            if (ReceiptDetails.Count > 0)
            {
                var receipt = new Receipt
                {
                    CashierID = SessionManager.CurrentUser.UserId,
                    ReceiptDate = DateTime.Now,
                    AmountCollected = TotalAmount
                };

                receiptBLL.AddReceipt(receipt, ReceiptDetails.ToList());

                ReceiptDetails.Clear();
                TotalAmount = 0;
            }
        }

        private void ClearFields()
        {
            SelectedProduct = null;
            Quantity = 0;
        }
    }
}
