using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket.Models.EntityLayer
{
    public class Stock : BasePropertyChanged
    {
        private int stockID;
        private int productID;
        private decimal quantity;
        private string unitOfMeasure;
        private DateTime supplyDate;
        private DateTime expirationDate;
        private decimal purchasePrice;
        private decimal salePrice;
        private bool? isActive; 
        private string productName;

        public int StockID
        {
            get { return stockID; }
            set
            {
                stockID = value;
                NotifyPropertyChanged(nameof(StockID));
            }
        }

        public int ProductID
        {
            get { return productID; }
            set
            {
                productID = value;
                NotifyPropertyChanged(nameof(ProductID));
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

        public string UnitOfMeasure
        {
            get { return unitOfMeasure; }
            set
            {
                unitOfMeasure = value;
                NotifyPropertyChanged(nameof(UnitOfMeasure));
            }
        }

        public DateTime SupplyDate
        {
            get { return supplyDate; }
            set
            {
                supplyDate = value;
                NotifyPropertyChanged(nameof(SupplyDate));
            }
        }

        public DateTime ExpirationDate
        {
            get { return expirationDate; }
            set
            {
                expirationDate = value;
                NotifyPropertyChanged(nameof(ExpirationDate));
            }
        }

        public decimal PurchasePrice
        {
            get { return purchasePrice; }
            set
            {
                purchasePrice = value;
                NotifyPropertyChanged(nameof(PurchasePrice));
            }
        }

        public decimal SalePrice
        {
            get { return salePrice; }
            set
            {
                salePrice = value;
                NotifyPropertyChanged(nameof(SalePrice));
            }
        }

        public bool? IsActive 
        {
            get { return isActive; }
            set
            {
                isActive = value;
                NotifyPropertyChanged(nameof(IsActive));
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
    }

}
