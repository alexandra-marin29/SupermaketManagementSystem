using System.Collections.ObjectModel;
using System.Windows.Input;
using Supermarket.Models.BusinessLogic;
using Supermarket.Models.EntityLayer;
using Supermarket.ViewModels.Commands;

namespace Supermarket.ViewModels
{
    public class ManufacturerVM : BasePropertyChanged
    {
        private ManufacturerBLL manufacturerBLL = new ManufacturerBLL();
        private Manufacturer selectedManufacturer;
        private string newManufacturerName;
        private string newCountryOfOrigin;

        public ObservableCollection<Manufacturer> Manufacturers { get; set; }
        public ICommand AddManufacturerCommand { get; }
        public ICommand EditManufacturerCommand { get; }
        public ICommand DeleteManufacturerCommand { get; }

        public ManufacturerVM()
        {
            Manufacturers = new ObservableCollection<Manufacturer>(manufacturerBLL.GetAllManufacturers());
            AddManufacturerCommand = new RelayCommand<object>(AddManufacturer);
            EditManufacturerCommand = new RelayCommand<object>(EditManufacturer);
            DeleteManufacturerCommand = new RelayCommand<object>(DeleteManufacturer);
        }

        public Manufacturer SelectedManufacturer
        {
            get { return selectedManufacturer; }
            set
            {
                selectedManufacturer = value;
                NotifyPropertyChanged(nameof(SelectedManufacturer));
                if (selectedManufacturer != null)
                {
                    NewManufacturerName = selectedManufacturer.ManufacturerName;
                    NewCountryOfOrigin = selectedManufacturer.CountryOfOrigin;
                }
            }
        }

        public string NewManufacturerName
        {
            get { return newManufacturerName; }
            set
            {
                newManufacturerName = value;
                NotifyPropertyChanged(nameof(NewManufacturerName));
            }
        }

        public string NewCountryOfOrigin
        {
            get { return newCountryOfOrigin; }
            set
            {
                newCountryOfOrigin = value;
                NotifyPropertyChanged(nameof(NewCountryOfOrigin));
            }
        }

        private void AddManufacturer(object parameter)
        {
            if (!string.IsNullOrEmpty(NewManufacturerName) && !string.IsNullOrEmpty(NewCountryOfOrigin))
            {
                Manufacturer newManufacturer = new Manufacturer
                {
                    ManufacturerName = NewManufacturerName,
                    CountryOfOrigin = NewCountryOfOrigin,
                    IsActive = true
                };
                manufacturerBLL.AddManufacturer(newManufacturer);
                Manufacturers.Add(newManufacturer);
                NewManufacturerName = string.Empty;
                NewCountryOfOrigin = string.Empty;
            }
        }

        private void EditManufacturer(object parameter)
        {
            if (SelectedManufacturer != null && !string.IsNullOrEmpty(NewManufacturerName) && !string.IsNullOrEmpty(NewCountryOfOrigin))
            {
                SelectedManufacturer.ManufacturerName = NewManufacturerName;
                SelectedManufacturer.CountryOfOrigin = NewCountryOfOrigin;
                manufacturerBLL.EditManufacturer(SelectedManufacturer);
                int index = Manufacturers.IndexOf(SelectedManufacturer);
                if (index >= 0)
                {
                    Manufacturers[index] = new Manufacturer
                    {
                        ManufacturerID = SelectedManufacturer.ManufacturerID,
                        ManufacturerName = SelectedManufacturer.ManufacturerName,
                        CountryOfOrigin = SelectedManufacturer.CountryOfOrigin,
                        IsActive = SelectedManufacturer.IsActive
                    };
                }
                NewManufacturerName = string.Empty;
                NewCountryOfOrigin = string.Empty;
                SelectedManufacturer = null;
            }
        }

        private void DeleteManufacturer(object parameter)
        {
            if (SelectedManufacturer != null)
            {
                manufacturerBLL.DeleteManufacturer(SelectedManufacturer.ManufacturerID);
                Manufacturers.Remove(SelectedManufacturer);
                SelectedManufacturer = null;
                NewManufacturerName = string.Empty;
                NewCountryOfOrigin = string.Empty;
            }
        }
    }
}
