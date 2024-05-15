using Supermarket.ViewModels;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace Supermarket.Views
{
    public class AdminWindowVM : INotifyPropertyChanged
    {
        public UserVM UserVM { get; set; }
        public CategoryVM CategoryVM { get; set; }
        public ProductVM ProductVM { get; set; }
        public StockVM StockVM { get; set; }
        public ManufacturerVM ManufacturerVM { get; set; }
        public ReportsVM ReportsVM { get; set; }

        public AdminWindowVM()
        {
            UserVM = new UserVM();
            CategoryVM = new CategoryVM();
            ProductVM = new ProductVM();
            StockVM = new StockVM();
            ManufacturerVM = new ManufacturerVM();
            ReportsVM = new ReportsVM();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ObservableCollection<ReportType> ReportTypes
        {
            get { return ReportsVM.ReportTypes; }
        }

        public ReportType SelectedReportType
        {
            get { return ReportsVM.SelectedReportType; }
            set
            {
                ReportsVM.SelectedReportType = value;
                NotifyPropertyChanged(nameof(SelectedReportType));
            }
        }
    }
}
