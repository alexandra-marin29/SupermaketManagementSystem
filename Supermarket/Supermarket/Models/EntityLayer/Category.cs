﻿namespace Supermarket.Models.EntityLayer
{
    public class Category : BasePropertyChanged
    {
        private int categoryId;
        private string categoryName;
        private bool isActive;

        public int CategoryId
        {
            get { return categoryId; }
            set
            {
                categoryId = value;
                NotifyPropertyChanged(nameof(CategoryId));
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
