using System.Collections.ObjectModel;
using System.Windows.Input;
using Supermarket.Models.BusinessLogic;
using Supermarket.Models.EntityLayer;
using Supermarket.ViewModels.Commands;

namespace Supermarket.ViewModels
{
    public class CategoryVM : BasePropertyChanged
    {
        private CategoryBLL categoryBLL = new CategoryBLL();
        private Category selectedCategory;
        private string newCategoryName;

        public ObservableCollection<Category> Categories { get; set; }
        public ICommand AddCategoryCommand { get; }
        public ICommand EditCategoryCommand { get; }
        public ICommand DeleteCategoryCommand { get; }

        public CategoryVM()
        {
            Categories = new ObservableCollection<Category>(categoryBLL.GetAllCategories());
            AddCategoryCommand = new RelayCommand<object>(AddCategory);
            EditCategoryCommand = new RelayCommand<object>(EditCategory);
            DeleteCategoryCommand = new RelayCommand<object>(DeleteCategory);
        }

        public Category SelectedCategory
        {
            get { return selectedCategory; }
            set
            {
                selectedCategory = value;
                NotifyPropertyChanged(nameof(SelectedCategory));
                if (selectedCategory != null)
                {
                    NewCategoryName = selectedCategory.CategoryName;
                }
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

        private void AddCategory(object parameter)
        {
            if (!string.IsNullOrEmpty(NewCategoryName))
            {
                Category newCategory = new Category { CategoryName = NewCategoryName, IsActive = true };
                categoryBLL.AddCategory(newCategory);
                Categories.Add(newCategory);
                NewCategoryName = string.Empty;
            }
        }

        private void EditCategory(object parameter)
        {
            if (SelectedCategory != null && !string.IsNullOrEmpty(NewCategoryName))
            {
                SelectedCategory.CategoryName = NewCategoryName;
                categoryBLL.EditCategory(SelectedCategory);
                int index = Categories.IndexOf(SelectedCategory);
                Categories[index] = SelectedCategory;
            }
        }

        private void DeleteCategory(object parameter)
        {
            if (SelectedCategory != null)
            {
                categoryBLL.DeleteCategory(SelectedCategory.CategoryId);
                Categories.Remove(SelectedCategory);
                SelectedCategory = null;
                NewCategoryName = string.Empty;
            }
        }
    }
}
