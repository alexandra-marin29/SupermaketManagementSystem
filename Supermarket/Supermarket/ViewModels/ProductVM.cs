using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Supermarket.Models.BusinessLogic;
using Supermarket.Models.EntityLayer;
using Supermarket.ViewModels.Commands;

namespace Supermarket.ViewModels
{
    public class ProductVM : BasePropertyChanged
    {
        private ProductBLL productBLL = new ProductBLL();
        private CategoryBLL categoryBLL = new CategoryBLL();
        private ManufacturerBLL manufacturerBLL = new ManufacturerBLL();

        private Product selectedProduct;
        private string newProductName;
        private string newBarcode;
        private string newCategoryName;
        private string newManufacturerName;
        private Category newCategory;
        private Manufacturer newManufacturer;

        public ObservableCollection<Product> Products { get; set; }
        public ObservableCollection<Category> Categories { get; set; }
        public ObservableCollection<Manufacturer> Manufacturers { get; set; }

        public ICommand AddProductCommand { get; }
        public ICommand EditProductCommand { get; }
        public ICommand DeleteProductCommand { get; }

        public ProductVM()
        {
            Products = new ObservableCollection<Product>(productBLL.GetAllProducts());
            Categories = new ObservableCollection<Category>(categoryBLL.GetAllCategories());
            Manufacturers = new ObservableCollection<Manufacturer>(manufacturerBLL.GetAllManufacturers());

            AddProductCommand = new RelayCommand<object>(AddProduct);
            EditProductCommand = new RelayCommand<object>(EditProduct);
            DeleteProductCommand = new RelayCommand<object>(DeleteProduct);
        }

        public Product SelectedProduct
        {
            get { return selectedProduct; }
            set
            {
                selectedProduct = value;
                NotifyPropertyChanged(nameof(SelectedProduct));
                if (selectedProduct != null)
                {
                    NewProductName = selectedProduct.ProductName;
                    NewBarcode = selectedProduct.Barcode;
                    NewCategory = Categories.FirstOrDefault(c => c.CategoryID == selectedProduct.CategoryID);
                    NewManufacturer = Manufacturers.FirstOrDefault(m => m.ManufacturerID == selectedProduct.ManufacturerID);
                }
            }
        }

        public string NewProductName
        {
            get { return newProductName; }
            set
            {
                newProductName = value;
                NotifyPropertyChanged(nameof(NewProductName));
            }
        }

        public string NewBarcode
        {
            get { return newBarcode; }
            set
            {
                newBarcode = value;
                NotifyPropertyChanged(nameof(NewBarcode));
            }
        }

        public string NewCategoryName
        {
            get { return newCategoryName; }
            set
            {
                newCategoryName = value;
                NotifyPropertyChanged(nameof(NewCategoryName));
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

        public Category NewCategory
        {
            get { return newCategory; }
            set
            {
                newCategory = value;
                NotifyPropertyChanged(nameof(NewCategory));
            }
        }

        public Manufacturer NewManufacturer
        {
            get { return newManufacturer; }
            set
            {
                newManufacturer = value;
                NotifyPropertyChanged(nameof(NewManufacturer));
            }
        }

        private void AddProduct(object parameter)
        {
            if (!string.IsNullOrEmpty(NewProductName) && !string.IsNullOrEmpty(NewBarcode))
            {
                if (NewCategory == null)
                {
                    NewCategory = new Category { CategoryName = NewCategoryName, IsActive = true };
                    categoryBLL.AddCategory(NewCategory);
                    Categories.Add(NewCategory);
                }

                if (NewManufacturer == null)
                {
                    NewManufacturer = new Manufacturer { ManufacturerName = NewManufacturerName, IsActive = true };
                    manufacturerBLL.AddManufacturer(NewManufacturer);
                    Manufacturers.Add(NewManufacturer);
                }

                Product newProduct = new Product
                {
                    ProductName = NewProductName,
                    Barcode = NewBarcode,
                    CategoryID = NewCategory.CategoryID,
                    ManufacturerID = NewManufacturer.ManufacturerID,
                    IsActive = true
                };

                productBLL.AddProduct(newProduct);
                Products.Add(newProduct);
                NewProductName = string.Empty;
                NewBarcode = string.Empty;
                NewCategory = null;
                NewManufacturer = null;
            }
        }

        private void EditProduct(object parameter)
        {
            if (SelectedProduct != null && !string.IsNullOrEmpty(NewProductName) && !string.IsNullOrEmpty(NewBarcode))
            {
                if (NewCategory == null)
                {
                    NewCategory = new Category { CategoryName = NewCategoryName, IsActive = true };
                    categoryBLL.AddCategory(NewCategory);
                    Categories.Add(NewCategory);
                }

                if (NewManufacturer == null)
                {
                    NewManufacturer = new Manufacturer { ManufacturerName = NewManufacturerName, IsActive = true };
                    manufacturerBLL.AddManufacturer(NewManufacturer);
                    Manufacturers.Add(NewManufacturer);
                }

                SelectedProduct.ProductName = NewProductName;
                SelectedProduct.Barcode = NewBarcode;
                SelectedProduct.CategoryID = NewCategory.CategoryID;
                SelectedProduct.ManufacturerID = NewManufacturer.ManufacturerID;

                productBLL.EditProduct(SelectedProduct);
                int index = Products.IndexOf(SelectedProduct);
                if (index >= 0)
                {
                    Products[index] = new Product
                    {
                        ProductID = SelectedProduct.ProductID,
                        ProductName = SelectedProduct.ProductName,
                        Barcode = SelectedProduct.Barcode,
                        CategoryID = SelectedProduct.CategoryID,
                        ManufacturerID = SelectedProduct.ManufacturerID,
                        IsActive = SelectedProduct.IsActive
                    };
                }
                NewProductName = string.Empty;
                NewBarcode = string.Empty;
                NewCategory = null;
                NewManufacturer = null;
                SelectedProduct = null;
            }
        }

        private void DeleteProduct(object parameter)
        {
            if (SelectedProduct != null)
            {
                productBLL.DeleteProduct(SelectedProduct.ProductID);
                Products.Remove(SelectedProduct);
                SelectedProduct = null;
                NewProductName = string.Empty;
                NewBarcode = string.Empty;
                NewCategory = null;
                NewManufacturer = null;
            }
        }
    }
}
