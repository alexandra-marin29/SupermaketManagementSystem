using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
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

        public ObservableCollection<Stock> Stocks { get; set; }
        public ObservableCollection<Product> Products { get; set; }

        public ICommand AddStockCommand { get; }
        public ICommand EditStockCommand { get; }
        public ICommand DeleteStockCommand { get; }

        public StockVM()
        {
            Stocks = new ObservableCollection<Stock>(stockBLL.GetAllStocks());
            Products = new ObservableCollection<Product>(productBLL.GetAllProducts());

            AddStockCommand = new RelayCommand<object>(AddStock);
            EditStockCommand = new RelayCommand<object>(EditStock);
            DeleteStockCommand = new RelayCommand<object>(DeleteStock);

            // Retrieve commercial markup from configuration
            commercialMarkup = decimal.Parse(ConfigurationManager.AppSettings["CommercialMarkup"]);

            IsAddingNewStock = true; // Initially, set to true when adding a new stock
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
                    IsAddingNewStock = false; // Set to false when editing an existing stock
                }
                else
                {
                    IsAddingNewStock = true; // Set to true when adding a new stock
                }
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
                newExpirationDate = value;
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
                // Automatically calculate the sale price
                NewSalePrice = newPurchasePrice * (1 + commercialMarkup);
            }
        }

        public decimal NewSalePrice
        {
            get { return newSalePrice; }
            set
            {
                newSalePrice = value;
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

        private void AddStock(object parameter)
        {
            if (NewProduct != null && NewPurchasePrice > 0)
            {
                Stock newStock = new Stock
                {
                    ProductID = NewProduct.ProductID,
                    Quantity = NewQuantity,
                    UnitOfMeasure = NewUnitOfMeasure,
                    SupplyDate = NewSupplyDate,
                    ExpirationDate = NewExpirationDate,
                    PurchasePrice = NewPurchasePrice,
                    SalePrice = NewSalePrice,
                    IsActive = true
                };

                stockBLL.AddStock(newStock);
                Stocks.Add(newStock);
                ClearFields();
            }
        }

        private void EditStock(object parameter)
        {
            if (SelectedStock != null && NewSalePrice >= NewPurchasePrice)
            {
                SelectedStock.Quantity = NewQuantity;
                SelectedStock.UnitOfMeasure = NewUnitOfMeasure;
                SelectedStock.SupplyDate = NewSupplyDate;
                SelectedStock.ExpirationDate = NewExpirationDate;
                SelectedStock.SalePrice = NewSalePrice;

                stockBLL.EditStock(SelectedStock);
                int index = Stocks.IndexOf(SelectedStock);
                Stocks[index] = SelectedStock;
                ClearFields();
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

        private void ClearFields()
        {
            NewQuantity = 0;
            NewUnitOfMeasure = string.Empty;
            NewSupplyDate = DateTime.Now;
            NewExpirationDate = DateTime.Now;
            NewPurchasePrice = 0;
            NewSalePrice = 0;
            NewProduct = null;
            IsAddingNewStock = true; // Reset to true for adding new stock
        }
    }
}
