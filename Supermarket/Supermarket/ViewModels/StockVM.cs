using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using Supermarket.Models.BusinessLogic;
using Supermarket.Models.EntityLayer;
using Supermarket.ViewModels.Commands;

namespace Supermarket.ViewModels
{
    public class StockVM : BasePropertyChanged
    {
        private StockBLL stockBLL = new StockBLL();
        private ProductBLL productBLL = new ProductBLL();

        private Stock selectedStock;
        private decimal newQuantity;
        private string newUnitOfMeasure;
        private DateTime newSupplyDate;
        private DateTime newExpirationDate;
        private decimal newPurchasePrice;
        private decimal newSalePrice;
        private Product newProduct;
        private decimal commercialMarkup;
        private bool isAddingNewStock;
        private bool isPurchasePriceReadOnly;
        public bool IsQuantityEditable { get; set; } = true;
        public bool IsUnitOfMeasureEditable { get; set; } = true;
        public bool IsSupplyDateEditable { get; set; } = true;
        public bool IsExpirationDateEditable { get; set; } = true;
        public bool IsPurchasePriceEditable { get; set; } = true;
        public bool IsSalePriceEditable { get; set; } = true;


        public ObservableCollection<Stock> Stocks { get; set; }
        public ObservableCollection<Product> Products { get; set; }
        public ObservableCollection<string> UnitOfMeasures { get; set; } 

        public ICommand AddStockCommand { get; }
        public ICommand EditStockCommand { get; }
        public ICommand DeleteStockCommand { get; }

        public StockVM()
        {
            Stocks = new ObservableCollection<Stock>(stockBLL.GetAllStocks());
            foreach (var stock in Stocks)
            {
                stock.ProductName = productBLL.GetProductById(stock.ProductID).ProductName;
            }

            Products = new ObservableCollection<Product>(productBLL.GetAllProducts());
            UnitOfMeasures = new ObservableCollection<string> { "kg", "g", "l", "ml" };

            AddStockCommand = new RelayCommand<object>(AddStock);
            EditStockCommand = new RelayCommand<object>(EditStock);
            DeleteStockCommand = new RelayCommand<object>(DeleteStock);

            string markupString = ConfigurationManager.AppSettings["CommercialMarkup"];
            if (decimal.TryParse(markupString, out decimal markup))
            {
                commercialMarkup = markup / 100;
            }
            else
            {
                commercialMarkup = 0.20m;
            }

            NewSupplyDate = new DateTime(2024, 1, 1);
            NewExpirationDate = new DateTime(2024, 1, 1);
            IsAddingNewStock = true;

            IsQuantityEditable = true;
            IsUnitOfMeasureEditable = true;
            IsSupplyDateEditable = true;
            IsExpirationDateEditable = true;
            IsPurchasePriceEditable = true;
            IsSalePriceEditable = true;
        }

        private void ClearFields()
        {
            NewQuantity = 0;
            NewUnitOfMeasure = string.Empty;
            NewSupplyDate = new DateTime(2024, 1, 1);
            NewExpirationDate = new DateTime(2024, 1, 1);
            NewPurchasePrice = 0;
            NewSalePrice = 0;
            NewProduct = null;

            IsQuantityEditable = true;
            IsUnitOfMeasureEditable = true;
            IsSupplyDateEditable = true;
            IsExpirationDateEditable = true;
            IsPurchasePriceEditable = true;
            IsSalePriceEditable = true;

            NotifyPropertyChanged(nameof(IsQuantityEditable));
            NotifyPropertyChanged(nameof(IsUnitOfMeasureEditable));
            NotifyPropertyChanged(nameof(IsSupplyDateEditable));
            NotifyPropertyChanged(nameof(IsExpirationDateEditable));
            NotifyPropertyChanged(nameof(IsPurchasePriceEditable));
            NotifyPropertyChanged(nameof(IsSalePriceEditable));
        }


        public Stock SelectedStock
        {
            get { return selectedStock; }
            set
            {
                selectedStock = value;
                NotifyPropertyChanged(nameof(SelectedStock));
                if (selectedStock != null)
                {
                    NewQuantity = selectedStock.Quantity;
                    NewUnitOfMeasure = selectedStock.UnitOfMeasure;
                    NewSupplyDate = selectedStock.SupplyDate;
                    NewExpirationDate = selectedStock.ExpirationDate;
                    NewPurchasePrice = selectedStock.PurchasePrice;
                    NewSalePrice = selectedStock.SalePrice;
                    NewProduct = Products.FirstOrDefault(p => p.ProductID == selectedStock.ProductID);

                    IsQuantityEditable = false;
                    IsUnitOfMeasureEditable = false;
                    IsSupplyDateEditable = false;
                    IsExpirationDateEditable = false;
                    IsPurchasePriceEditable = false;
                    IsSalePriceEditable = true;
                }
                else
                {
                    IsQuantityEditable = true;
                    IsUnitOfMeasureEditable = true;
                    IsSupplyDateEditable = true;
                    IsExpirationDateEditable = true;
                    IsPurchasePriceEditable = true;
                    IsSalePriceEditable = true;
                }

                NotifyPropertyChanged(nameof(IsQuantityEditable));
                NotifyPropertyChanged(nameof(IsUnitOfMeasureEditable));
                NotifyPropertyChanged(nameof(IsSupplyDateEditable));
                NotifyPropertyChanged(nameof(IsExpirationDateEditable));
                NotifyPropertyChanged(nameof(IsPurchasePriceEditable));
                NotifyPropertyChanged(nameof(IsSalePriceEditable));
            }
        }



        public decimal NewQuantity
        {
            get { return newQuantity; }
            set
            {
                newQuantity = value;
                NotifyPropertyChanged(nameof(NewQuantity));
            }
        }

        public string NewUnitOfMeasure
        {
            get { return newUnitOfMeasure; }
            set
            {
                newUnitOfMeasure = value;
                NotifyPropertyChanged(nameof(NewUnitOfMeasure));
            }
        }

        public DateTime NewSupplyDate
        {
            get { return newSupplyDate; }
            set
            {
                newSupplyDate = value;
                NotifyPropertyChanged(nameof(NewSupplyDate));
            }
        }

        public DateTime NewExpirationDate
        {
            get { return newExpirationDate; }
            set
            {
                if (value < NewSupplyDate)
                {
                    MessageBox.Show("Expiration date cannot be earlier than supply date.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    newExpirationDate = NewSupplyDate;  
                }
                else
                {
                    newExpirationDate = value;
                }
                NotifyPropertyChanged(nameof(NewExpirationDate));
            }
        }

        public decimal NewPurchasePrice
        {
            get { return newPurchasePrice; }
            set
            {
                newPurchasePrice = value;
                NotifyPropertyChanged(nameof(NewPurchasePrice));
                NewSalePrice = newPurchasePrice * (1 + commercialMarkup);
            }
        }

        public decimal NewSalePrice
        {
            get { return newSalePrice; }
            set
            {
                newSalePrice = value;
                if (newSalePrice < NewPurchasePrice)
                {
                    MessageBox.Show("Sale price cannot be less than purchase price.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    NewSalePrice = NewPurchasePrice * (1 + commercialMarkup);
                }
                NotifyPropertyChanged(nameof(NewSalePrice));
            }
        }

        public Product NewProduct
        {
            get { return newProduct; }
            set
            {
                newProduct = value;
                NotifyPropertyChanged(nameof(NewProduct));
            }
        }

        public bool IsAddingNewStock
        {
            get { return isAddingNewStock; }
            set
            {
                isAddingNewStock = value;
                NotifyPropertyChanged(nameof(IsAddingNewStock));
            }
        }

        public bool IsPurchasePriceReadOnly
        {
            get { return isPurchasePriceReadOnly; }
            set
            {
                isPurchasePriceReadOnly = value;
                NotifyPropertyChanged(nameof(IsPurchasePriceReadOnly));
            }
        }

        private void AddStock(object parameter)
        {
            if (NewProduct != null && NewPurchasePrice > 0)
            {
                if (NewExpirationDate < NewSupplyDate)
                {
                    MessageBox.Show("Expiration date cannot be earlier than supply date.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                Stock newStock = new Stock
                {
                    ProductID = NewProduct.ProductID,
                    Quantity = NewQuantity,
                    UnitOfMeasure = NewUnitOfMeasure,
                    SupplyDate = NewSupplyDate,
                    ExpirationDate = NewExpirationDate,
                    PurchasePrice = NewPurchasePrice,
                    SalePrice = NewSalePrice,
                    IsActive = true,
                    ProductName = NewProduct.ProductName
                };

                stockBLL.AddStock(newStock);
                newStock.ProductName = productBLL.GetProductById(newStock.ProductID).ProductName;
                Stocks.Add(newStock);
                ClearFields();
            }
        }

        private void EditStock(object parameter)
        {
            if (SelectedStock != null && NewSalePrice >= NewPurchasePrice)
            {
                if (NewExpirationDate < NewSupplyDate)
                {
                    MessageBox.Show("Expiration date cannot be earlier than supply date.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                SelectedStock.Quantity = NewQuantity;
                SelectedStock.UnitOfMeasure = NewUnitOfMeasure;
                SelectedStock.SupplyDate = NewSupplyDate;
                SelectedStock.ExpirationDate = NewExpirationDate;
                SelectedStock.SalePrice = NewSalePrice;
                SelectedStock.ProductName = NewProduct.ProductName;

                stockBLL.EditStock(SelectedStock);
                int index = Stocks.IndexOf(SelectedStock);
                Stocks[index] = SelectedStock;
                ClearFields();
            }
            else if (NewSalePrice < NewPurchasePrice)
            {
                MessageBox.Show("Sale price cannot be less than purchase price.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteStock(object parameter)
        {
            if (SelectedStock != null)
            {
                stockBLL.DeleteStock(SelectedStock.StockID);
                Stocks.Remove(SelectedStock);
                ClearFields();
            }
        }

        

        private bool ValidateProductInputs(string productName, string barcode, string categoryName)
        {
            if (string.IsNullOrWhiteSpace(productName) || !Regex.IsMatch(productName, @"^[a-zA-Z]+$"))
            {
                MessageBox.Show("Product name must be non-empty and contain only letters.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(barcode) || !Regex.IsMatch(barcode, @"^[0-9]+$"))
            {
                MessageBox.Show("Barcode must be non-empty and contain only digits.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(categoryName) || !Regex.IsMatch(categoryName, @"^[a-zA-Z]+$"))
            {
                MessageBox.Show("Category name must be non-empty and contain only letters.");
                return false;
            }

            return true;
        }
    }
}
