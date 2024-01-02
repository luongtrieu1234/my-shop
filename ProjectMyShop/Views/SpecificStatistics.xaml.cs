using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
using LiveCharts;
using LiveCharts.Wpf;
using ProjectMyShop.BUS;
using ProjectMyShop.DTO;

namespace ProjectMyShop.Views
{
    /// <summary>
    /// Interaction logic for SpecificStatistics.xaml
    /// </summary>
    public partial class SpecificStatistics : Page
    {
        public SpecificStatistics(Statistics srcPage)
        {
            InitializeComponent();

            _statisticsPage = srcPage;
            _advancedPage = new AdvancedStatistics(srcPage);

            _statisticsBUS = new StatisticsBUS();
            _categoryBUS = new CategoryBUS();
            _phoneBUS = new ProductBUS();

            categories = _categoryBUS.getCategoryList();
            categoriesCombobox.ItemsSource = categories;

            if (categories.Count() > 0)
                phones = _phoneBUS.getPhonesAccordingToSpecificCategory(categories[categoriesFigureIndex].ID);
            else
                phones = new List<Product> { };

            productCombobox.ItemsSource = phones;

            statisticsCombobox.ItemsSource = statisticsFigureValues;
            statisticsCombobox.SelectedIndex = statisticsFigureIndex;

            bargraphCombobox.ItemsSource = figureValues;
            bargraphCombobox.SelectedIndex = bargraphFigureIndex;

            statisticsDatePicker.SelectedDate = selectedDate;

            chartTabControl.SelectedIndex = tabSelectedIndex;

            DataContext = this;
        }

        public void getAdvancedStatistic(AdvancedStatistics srcAdvancedStatistics)
        {
            _advancedPage = srcAdvancedStatistics;
        }

        private StatisticsBUS _statisticsBUS;
        private CategoryBUS _categoryBUS;
        private ProductBUS _phoneBUS;
        public int statisticsFigureIndex { get; set; } = 1;
        public int bargraphFigureIndex { get; set; } = 0;
        public int tabSelectedIndex { get; set; } = 0;
        public int categoriesFigureIndex { get; set; } = 0;
        public int productFigureIndex { get; set; } = 0;
        public DateTime selectedDate { get; set; } = DateTime.Now;
        public List<string> figureValues = new List<string>() { "Daily", "Weekly", "Monthly", "Yearly" };
        public List<string> statisticsFigureValues = new List<string>() { "General", "Specific", "Advanced" };
        public List<Category> categories;
        public List<Product> phones;
        private Statistics _statisticsPage;
        private AdvancedStatistics _advancedPage;

        Func<ChartPoint, string> labelPoint = chartPoint => string.Format("{0} ({1:P})", chartPoint.Y, chartPoint.Participation);
        public void configureBarGraphs()
        {
            switch (bargraphFigureIndex)
            {
                case 0:
                    if (phones.Count() > 0 && categories.Count() > 0)
                    {
                        var productResult = _statisticsBUS.getDailyQuantityOfSpecificProduct(phones[productFigureIndex].ID, categories[categoriesFigureIndex].ID, selectedDate);

                        var quantity = new ChartValues<int>();
                        var dates = new List<string>();

                        foreach (var item in productResult)
                        {
                            quantity.Add((int)item.Item2);
                            dates.Add(item.Item1.ToString());
                        }

                        var quantityCollection = new SeriesCollection()
                    {
                    new RowSeries
                    {
                        Title = "Quantity: ",
                        Values = quantity,
                    }
                    };


                        productBarGraph.AxisY.Clear();
                        productBarGraph.AxisY.Add(new LiveCharts.Wpf.Axis
                        {
                            Title = "Date",
                            Labels = dates
                        });

                        productBarGraph.AxisX.Clear();
                        productBarGraph.AxisX.Add(new LiveCharts.Wpf.Axis
                        {
                            Title = "Quantity",
                            LabelFormatter = x => x.ToString("N0")

                        });

                        productBarGraph.Series = quantityCollection;
                    }
                    break;
                case 1:
                    if (phones.Count() > 0 && categories.Count() > 0)
                    {
                        var weeklyProductResult = _statisticsBUS.getMonthlyQuantityOfSpecificProduct(phones[productFigureIndex].ID, categories[categoriesFigureIndex].ID, selectedDate);

                        var weeklyQuantity = new ChartValues<int>();
                        var weeks = new List<string>();

                        foreach (var item in weeklyProductResult)
                        {
                            weeklyQuantity.Add((int)item.Item2);
                            weeks.Add(item.Item1.ToString());
                        }

                        var weeklyQuantityCollection = new SeriesCollection()
                        {
                            new ColumnSeries
                            {
                                Title = "Quantity: ",
                                Values = weeklyQuantity,
                            }
                        };

                        productBarGraph.AxisX.Clear();
                        productBarGraph.AxisX.Add(new LiveCharts.Wpf.Axis
                        {
                            Title = "Week",
                            Labels = weeks
                        });

                        productBarGraph.AxisY.Clear();
                        productBarGraph.AxisY.Add(new LiveCharts.Wpf.Axis
                        {
                            Title = "Quantity",
                            LabelFormatter = x => x.ToString("N0")

                        });

                        productBarGraph.Series = weeklyQuantityCollection;
                    }
                    break;
                case 2:
                    if (phones.Count() > 0 && categories.Count() > 0)
                    {
                        var monthlyProductResult = _statisticsBUS.getMonthlyQuantityOfSpecificProduct(phones[productFigureIndex].ID, categories[categoriesFigureIndex].ID, selectedDate);

                        var monthlyQuantity = new ChartValues<int>();
                        var months = new List<string>();

                        foreach (var item in monthlyProductResult)
                        {
                            monthlyQuantity.Add((int)item.Item2);
                            months.Add(item.Item1.ToString());
                        }

                        var monthlyQuantityCollection = new SeriesCollection()
                    {
                    new ColumnSeries
                    {
                        Title = "Quantity: ",
                        Values = monthlyQuantity,
                    }
                    };


                        productBarGraph.AxisX.Clear();
                        productBarGraph.AxisX.Add(new LiveCharts.Wpf.Axis
                        {
                            Title = "Month",
                            Labels = months
                        });

                        productBarGraph.AxisY.Clear();
                        productBarGraph.AxisY.Add(new LiveCharts.Wpf.Axis
                        {
                            Title = "Quantity",
                            LabelFormatter = x => x.ToString("N0")

                        });

                        productBarGraph.Series = monthlyQuantityCollection;
                    }
                    break;
                case 3:
                    if (phones.Count() > 0 && categories.Count() > 0)
                    {
                        var yearlyProductResult = _statisticsBUS.getYearlyQuantityOfSpecificProduct(phones[productFigureIndex].ID, categories[categoriesFigureIndex].ID);

                        var yearlyQuantity = new ChartValues<int>();
                        var years = new List<string>();

                        foreach (var item in yearlyProductResult)
                        {
                            yearlyQuantity.Add((int)item.Item2);
                            years.Add(item.Item1.ToString());
                        }

                        var yearlyQuantityCollection = new SeriesCollection()
                    {
                    new ColumnSeries
                    {
                        Title = "Quantity: ",
                        Values = yearlyQuantity,
                    }
                    };
                        productBarGraph.AxisX.Clear();
                        productBarGraph.AxisX.Add(new LiveCharts.Wpf.Axis
                        {
                            Title = "Month",
                            Labels = years
                        });

                        productBarGraph.AxisY.Clear();
                        productBarGraph.AxisY.Add(new LiveCharts.Wpf.Axis
                        {
                            Title = "Quantity",
                            LabelFormatter = x => x.ToString("N0")

                        });

                        productBarGraph.Series = yearlyQuantityCollection;
                    }
                    break;
            }
        }

        public void configurePieChart()
        {
            if (phones.Count() > 0 && categories.Count() > 0)
            {
                var phoneResult = _statisticsBUS.getPhoneQuantityInCategory(categories[categoriesFigureIndex].ID);

                var phoneQuantityCollection = new SeriesCollection();

                foreach (var item in phoneResult)
                {
                    phoneQuantityCollection.Add(new PieSeries
                    {
                        Title = item.Item1.ToString(),
                        Values = new ChartValues<double> { Convert.ToDouble((int)item.Item2) },
                        DataLabels = true,
                        //LabelPoint = labelPoint,
                    });
                }

                productPieChart.Series = phoneQuantityCollection;
            }
        }

        private void categoriesCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            phones = _phoneBUS.getPhonesAccordingToSpecificCategory(categories[categoriesFigureIndex].ID);
            productCombobox.ItemsSource = phones;

            configurePieChart();

            if (phones.Count > 0)
            {
                productFigureIndex = 0;
                productCombobox.SelectedIndex = productFigureIndex;
            }
        }

        private void productCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (productFigureIndex != -1)
            {
                configureBarGraphs();
            }
        }

        private void bargraphCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (categoriesFigureIndex != -1 && productFigureIndex != -1)
            {
                configureBarGraphs();
            }
        }

        private void statisticsDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            configureBarGraphs();
        }

        private void statisticsCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (statisticsFigureIndex)
            {
                case 0:
                    NavigationService.Navigate(_statisticsPage);
                    statisticsFigureIndex = 1;
                    statisticsCombobox.SelectedIndex = statisticsFigureIndex;
                    break;
                case 1:
                    break;
                case 2:
                    NavigationService.Navigate(_advancedPage);
                    statisticsFigureIndex = 1;
                    statisticsCombobox.SelectedIndex = statisticsFigureIndex;
                    break;
                default:
                    break;
            }
        }

        private void chartTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (tabSelectedIndex)
            {
                case 0:
                    configureBarGraphs();
                    break;
                case 1:
                    configurePieChart();
                    break;
            }
        }
    }
}
