using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ProjectMyShop.DTO;
using ProjectMyShop.BUS;

namespace ProjectMyShop.Views
{
    /// <summary>
    /// Interaction logic for AdvancedStatistics.xaml
    /// </summary>
    public partial class AdvancedStatistics : Page
    {
        public AdvancedStatistics(Statistics srcPage)
        {
            InitializeComponent();

            _statisticsPage = srcPage;

            statisticsDatePicker.SelectedDate = selectedDate;

            statisticsCombobox.ItemsSource = statisticsFigureValues;
            statisticsCombobox.SelectedIndex = statisticsFigureIndex;

            timeCombobox.ItemsSource = figureValues;
            timeCombobox.SelectedIndex = figureIndex;

            Products = _ProductBUS.getBestSellingProductsInWeek(selectedDate);

            for (int i = 0; i < Products.Count(); i++)
            {
               System.Diagnostics.Debug.WriteLine(Products[i].ProductName);
            }

            ProductDataGrid.ItemsSource = Products;

            DataContext = this;
        }

        public void getSpecificStatistic(SpecificStatistics srcSpecificStatistics)
        {
            _specificPage = srcSpecificStatistics;
        }
        public DateTime selectedDate { get; set; } = DateTime.Now;
        public int figureIndex { get; set; } = 0;
        public List<string> figureValues = new List<string>() {"In Week", "In Month", "In Year" };
        public List<string> statisticsFigureValues = new List<string>() { "General", "Specific", "Advanced" };
        public int statisticsFigureIndex { get; set; } = 2;
        public List<BestSellingProduct> Products;
        private Statistics _statisticsPage;
        private SpecificStatistics _specificPage;
        ProductBUS _ProductBUS = new ProductBUS();

        private void statisticsCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (statisticsFigureIndex)
            {
                case 0:
                    NavigationService.Navigate(_statisticsPage);
                    statisticsFigureIndex = 2;
                    statisticsCombobox.SelectedIndex = statisticsFigureIndex;
                    break;
                case 1:
                    NavigationService.Navigate(_specificPage);
                    statisticsFigureIndex = 2;
                    statisticsCombobox.SelectedIndex = statisticsFigureIndex;
                    break;
                case 2:
                    break;
                default:
                    break;
            }
        }

        public void configureBestSellingProductsDataGrid()
        {
            switch (figureIndex)
            {
                case 0:
                    Products = _ProductBUS.getBestSellingProductsInWeek(selectedDate);
                    ProductDataGrid.ItemsSource = Products;
                    break;
                case 1:
                    Products = _ProductBUS.getBestSellingProductsInMonth(selectedDate);
                    ProductDataGrid.ItemsSource = Products;
                    break;
                case 2:
                    Products = _ProductBUS.getBestSellingProductsInYear(selectedDate);
                    ProductDataGrid.ItemsSource = Products;
                    break;
                default:
                    Products = _ProductBUS.getBestSellingProductsInWeek(selectedDate);
                    ProductDataGrid.ItemsSource = Products;
                    break;
            }
        }

        private void timeCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            configureBestSellingProductsDataGrid();
        }

        private void statisticsDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            configureBestSellingProductsDataGrid();
        }
    }
}
