using System;

namespace Supermarket.Models.EntityLayer
{
    public class ReceiptDetail : BasePropertyChanged
    {
        private int receiptDetailID;
        private int receiptID;
        private int productID;
        private decimal quantity;
        private decimal subtotal;
        private string productName;

        public int ReceiptDetailID
        {
            get { return receiptDetailID; }
            set
            {
                receiptDetailID = value;
                NotifyPropertyChanged(nameof(ReceiptDetailID));
            }
        }

        public int ReceiptID
        {
            get { return receiptID; }
            set
            {
                receiptID = value;
                NotifyPropertyChanged(nameof(ReceiptID));
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

        public decimal Subtotal
        {
            get { return subtotal; }
            set
            {
                subtotal = value;
                NotifyPropertyChanged(nameof(Subtotal));
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
