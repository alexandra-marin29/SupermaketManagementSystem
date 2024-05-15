using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Supermarket.Models.EntityLayer
{
    public class Manufacturer : BasePropertyChanged
    {
        private int manufacturerId;
        private string manufacturerName;
        private string countryOfOrigin;
        private bool isActive;

        public int ManufacturerID
        {
            get { return manufacturerId; }
            set
            {
                manufacturerId = value;
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

        public string CountryOfOrigin
        {
            get { return countryOfOrigin; }
            set
            {
                countryOfOrigin = value;
                NotifyPropertyChanged(nameof(CountryOfOrigin));
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

