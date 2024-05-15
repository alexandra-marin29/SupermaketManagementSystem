using System;
using System.ComponentModel;

namespace Supermarket.Models.EntityLayer
{
    public class Receipt : BasePropertyChanged
    {
        private int receiptID;
        private int cashierID;
        private DateTime receiptDate;
        private decimal amountCollected;

        public int ReceiptID
        {
            get { return receiptID; }
            set
            {
                receiptID = value;
                NotifyPropertyChanged(nameof(ReceiptID));
            }
        }

        public int CashierID
        {
            get { return cashierID; }
            set
            {
                cashierID = value;
                NotifyPropertyChanged(nameof(CashierID));
            }
        }

        public DateTime ReceiptDate
        {
            get { return receiptDate; }
            set
            {
                receiptDate = value;
                NotifyPropertyChanged(nameof(ReceiptDate));
            }
        }

        public decimal AmountCollected
        {
            get { return amountCollected; }
            set
            {
                amountCollected = value;
                NotifyPropertyChanged(nameof(AmountCollected));
            }
        }
    }
}
