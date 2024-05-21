using System;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Windows;
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

        public void RefreshManufacturers()
        {
            Manufacturers.Clear();
            foreach (var manufacturer in manufacturerBLL.GetAllManufacturers())
            {
                Manufacturers.Add(manufacturer);
            }
        }

        private void AddManufacturer(object parameter)
        {
            if (ValidateManufacturerInputs(NewManufacturerName, NewCountryOfOrigin))
            {
                try
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
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void EditManufacturer(object parameter)
        {
            if (SelectedManufacturer != null && ValidateManufacturerInputs(NewManufacturerName, NewCountryOfOrigin))
            {
                string originalManufacturerName = SelectedManufacturer.ManufacturerName;
                string originalCountryOfOrigin = SelectedManufacturer.CountryOfOrigin; 

                try
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
                catch (Exception ex)
                {
                    if (SelectedManufacturer != null)
                    {
                        SelectedManufacturer.ManufacturerName = originalManufacturerName;
                        SelectedManufacturer.CountryOfOrigin = originalCountryOfOrigin;
                    }
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void DeleteManufacturer(object parameter)
        {
            if (SelectedManufacturer != null)
            {
                try
                {
                    manufacturerBLL.DeleteManufacturer(SelectedManufacturer.ManufacturerID);
                    Manufacturers.Remove(SelectedManufacturer);
                    SelectedManufacturer = null;
                    NewManufacturerName = string.Empty;
                    NewCountryOfOrigin = string.Empty;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private bool ValidateManufacturerInputs(string manufacturerName, string countryOfOrigin)
        {
            if (string.IsNullOrWhiteSpace(manufacturerName) || !Regex.IsMatch(manufacturerName, @"^[a-zA-Z0-9\s]+$"))
            {
                MessageBox.Show("Manufacturer name must be non-empty and contain only letters, digits, and spaces.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(countryOfOrigin) || !Regex.IsMatch(countryOfOrigin, @"^[a-zA-Z\s]+$"))
            {
                MessageBox.Show("Country of origin must be non-empty and contain only letters and spaces.");
                return false;
            }

            return true;
        }

    }
}
