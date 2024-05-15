﻿using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
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

        public ObservableCollection<Stock> Stocks { get; set; }
        public ObservableCollection<Product> Products { get; set; }

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
                    IsAddingNewStock = false; 
                    IsPurchasePriceReadOnly = true; 
                }
                else
                {
                    IsAddingNewStock = true; 
                    IsPurchasePriceReadOnly = false; 
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

        private void ClearFields()
        {
            NewQuantity = 0;
            NewUnitOfMeasure = string.Empty;
            NewSupplyDate = new DateTime(2024, 1, 1);
            NewExpirationDate = new DateTime(2024, 1, 1);
            NewPurchasePrice = 0;
            NewSalePrice = 0;
            NewProduct = null;
            IsAddingNewStock = true; 
            IsPurchasePriceReadOnly = false; 
        }
    }
}
