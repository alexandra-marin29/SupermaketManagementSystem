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
        private ObservableCollection<string> filteredBarcodes;
        private ObservableCollection<Manufacturer> filteredManufacturers;
        private ObservableCollection<Category> filteredCategories;

        private Product selectedProduct;
        private ReceiptDetail selectedReceiptDetail;
        private string quantity;
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
            ProductNames = new ObservableCollection<string>(productBLL.GetProductsInStock().Select(p => p.ProductName).Distinct());
            Barcodes = new ObservableCollection<string>(productBLL.GetProductsInStock().Select(p => p.Barcode).Distinct());
            Manufacturers = new ObservableCollection<Manufacturer>(manufacturerBLL.GetAllManufacturers());
            Categories = new ObservableCollection<Category>(categoryBLL.GetAllCategories());

            FilteredBarcodes = new ObservableCollection<string>(Barcodes);
            FilteredManufacturers = new ObservableCollection<Manufacturer>(Manufacturers);
            FilteredCategories = new ObservableCollection<Category>(Categories);

            SearchProductsCommand = new RelayCommand<object>(SearchProducts);
            AddToReceiptCommand = new RelayCommand<object>(AddToReceipt);
            FinalizeReceiptCommand = new RelayCommand<object>(FinalizeReceipt);
            RemoveFromReceiptCommand = new RelayCommand<object>(RemoveFromReceipt);
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

        public ObservableCollection<string> FilteredBarcodes
        {
            get { return filteredBarcodes; }
            set
            {
                filteredBarcodes = value;
                NotifyPropertyChanged(nameof(FilteredBarcodes));
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

        public ObservableCollection<Manufacturer> FilteredManufacturers
        {
            get { return filteredManufacturers; }
            set
            {
                filteredManufacturers = value;
                NotifyPropertyChanged(nameof(FilteredManufacturers));
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

        public ObservableCollection<Category> FilteredCategories
        {
            get { return filteredCategories; }
            set
            {
                filteredCategories = value;
                NotifyPropertyChanged(nameof(FilteredCategories));
            }
        }

        public Product SelectedProduct
        {
            get { return selectedProduct; }
            set
            {
                selectedProduct = value;
                NotifyPropertyChanged(nameof(SelectedProduct));
                if (selectedProduct != null)
                {
                    SelectedProductName = selectedProduct.ProductName;
                }
                UpdateFilteredLists();
            }
        }

        private string selectedProductName;
        public string SelectedProductName
        {
            get { return selectedProductName; }
            set
            {
                selectedProductName = value;
                NotifyPropertyChanged(nameof(SelectedProductName));
                UpdateFilteredLists();
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

        public string Quantity
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
        public ICommand RemoveFromReceiptCommand { get; }

        private void UpdateFilteredLists()
        {
            if (!string.IsNullOrEmpty(SelectedProductName))
            {
                var selectedProduct = productBLL.GetProductsInStock()
                    .FirstOrDefault(p => p.ProductName == SelectedProductName);

                if (selectedProduct != null)
                {
                    FilteredBarcodes = new ObservableCollection<string>(productBLL.GetProductsInStock()
                        .Where(p => p.ProductName == selectedProduct.ProductName)
                        .Select(p => p.Barcode)
                        .Distinct());

                    FilteredManufacturers = new ObservableCollection<Manufacturer>(manufacturerBLL.GetAllManufacturers()
                        .Where(m => m.ManufacturerID == selectedProduct.ManufacturerID));

                    FilteredCategories = new ObservableCollection<Category>(categoryBLL.GetAllCategories()
                        .Where(c => c.CategoryID == selectedProduct.CategoryID));
                }
            }
            else
            {
                FilteredBarcodes = new ObservableCollection<string>(Barcodes);
                FilteredManufacturers = new ObservableCollection<Manufacturer>(Manufacturers);
                FilteredCategories = new ObservableCollection<Category>(Categories);
            }
        }

        private void SearchProducts(object parameter)
        {
            Products.Clear();
            var productsList = productBLL.SearchProducts(SelectedProductName, Barcode, null, SelectedManufacturer?.ManufacturerID, SelectedCategory?.CategoryID)
                .Where(p => stockBLL.GetStocksByProductId(p.ProductID).Any(s => s.Quantity > 0 && s.IsActive == true));
            foreach (var product in productsList)
            {
                Products.Add(product);
            }
        }

        private void AddToReceipt(object parameter)
        {
            if (SelectedProduct != null && !string.IsNullOrEmpty(Quantity) && decimal.TryParse(Quantity, out decimal quantity))
            {
                var existingReceiptDetail = ReceiptDetails.FirstOrDefault(rd => rd.ProductID == SelectedProduct.ProductID);
                if (existingReceiptDetail != null)
                {
                    existingReceiptDetail.Quantity += quantity;
                    existingReceiptDetail.Subtotal += existingReceiptDetail.Quantity * existingReceiptDetail.Subtotal / (existingReceiptDetail.Quantity - quantity);
                }
                else
                {
                    var stocks = stockBLL.GetStocksByProductId(SelectedProduct.ProductID)
                        .Where(s => s.ExpirationDate > DateTime.Now && s.Quantity > 0)
                        .OrderBy(s => s.SupplyDate)
                        .ToList();

                    decimal remainingQuantity = quantity;
                    decimal subtotal = 0;

                    foreach (var stock in stocks)
                    {
                        if (remainingQuantity <= 0)
                            break;

                        decimal deductedQuantity = Math.Min(remainingQuantity, stock.Quantity);
                        remainingQuantity -= deductedQuantity;
                        subtotal += deductedQuantity * stock.SalePrice;
                    }

                    if (remainingQuantity > 0)
                    {
                        MessageBox.Show("Insufficient stock.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    var receiptDetail = new ReceiptDetail
                    {
                        ProductID = SelectedProduct.ProductID,
                        ProductName = SelectedProduct.ProductName,
                        Quantity = quantity,
                        Subtotal = subtotal
                    };

                    ReceiptDetails.Add(receiptDetail);
                }

                TotalAmount = ReceiptDetails.Sum(rd => rd.Subtotal);
                ClearFields();
            }
            else
            {
                MessageBox.Show("Invalid quantity format.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RemoveFromReceipt(object parameter)
        {
            if (SelectedReceiptDetail != null)
            {
                ReceiptDetails.Remove(SelectedReceiptDetail);
                TotalAmount = ReceiptDetails.Sum(rd => rd.Subtotal);
                ClearFields();
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

                // Adjust stock quantities
                foreach (var detail in ReceiptDetails)
                {
                    var stocks = stockBLL.GetStocksByProductId(detail.ProductID)
                        .Where(s => s.ExpirationDate > DateTime.Now && s.Quantity > 0)
                        .OrderBy(s => s.SupplyDate)
                        .ToList();

                    decimal remainingQuantity = detail.Quantity;
                    foreach (var stock in stocks)
                    {
                        if (remainingQuantity <= 0)
                            break;

                        decimal deductedQuantity = Math.Min(remainingQuantity, stock.Quantity);
                        remainingQuantity -= deductedQuantity;
                        stock.Quantity -= deductedQuantity;

                        if (stock.Quantity == 0)
                        {
                            stock.IsActive = false;
                        }

                        stockBLL.EditStock(stock);
                    }

                    if (remainingQuantity > 0)
                    {
                        MessageBox.Show("Insufficient stock for product: " + detail.ProductName, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }

                ReceiptDetails.Clear();
                TotalAmount = 0;
            }
        }

        private void ClearFields()
        {
            SelectedProduct = null;
            Quantity = string.Empty;
        }
    }
}
