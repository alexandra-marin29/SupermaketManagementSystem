using System;

namespace Supermarket.Models.EntityLayer
{
    public class Product : BasePropertyChanged
    {
        private int productID;
        private string productName;
        private string barcode;
        private int? categoryID;
        private int? manufacturerID;
        private string manufacturerName; 
        private string categoryName; 
        private bool isActive;

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

        public string Barcode
        {
            get { return barcode; }
            set
            {
                barcode = value;
                NotifyPropertyChanged(nameof(Barcode));
            }
        }

        public int? CategoryID
        {
            get { return categoryID; }
            set
            {
                categoryID = value;
                NotifyPropertyChanged(nameof(CategoryID));
            }
        }

        public int? ManufacturerID
        {
            get { return manufacturerID; }
            set
            {
                manufacturerID = value;
                NotifyPropertyChanged(nameof(ManufacturerID));
            }
        }

        public string ManufacturerName
        {
            get { return manufacturerName; }
            set
            {
                manufacturerName = value;
                NotifyPropertyChanged(nameof(ManufacturerName));
            }
        }

        public string CategoryName
        {
            get { return categoryName; }
            set
            {
                categoryName = value;
                NotifyPropertyChanged(nameof(CategoryName));
            }
        }

        public bool IsActive
        {
            get { return isActive; }
            set
            {
                isActive = value;
                NotifyPropertyChanged(nameof(IsActive));
            }
        }
    }
}
