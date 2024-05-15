﻿using Supermarket.ViewModels;
using System.ComponentModel;

namespace Supermarket.Views
{
    public class AdminWindowVM : INotifyPropertyChanged
    {
        public UserVM UserVM { get; set; }
        public CategoryVM CategoryVM { get; set; }
        public ManufacturerVM ManufacturerVM { get; set; }
        public ProductVM ProductVM { get; set; }
        public StockVM StockVM { get; set; }


        public AdminWindowVM()
        {
            UserVM = new UserVM();
            CategoryVM = new CategoryVM();
            ManufacturerVM = new ManufacturerVM();
            ProductVM = new ProductVM();
            StockVM = new StockVM();


        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
