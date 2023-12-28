using System;
using System.Collections.Generic;
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
using ProjectMyShop.Config;
using ProjectMyShop.Views;

namespace ProjectMyShop.Views
{
    /// <summary>
    /// Interaction logic for Statistics.xaml
    /// </summary>
    public partial class Statistics : Page
    {
        public Statistics()
        {
            InitializeComponent();

            _statisticsBUS = new StatisticsBUS();

            statisticsCombobox.ItemsSource = statisticsFigureValues;
            statisticsCombobox.SelectedIndex = statisticsFigureIndex;

            revenueCombobox.ItemsSource = figureValues;
            revenueCombobox.SelectedIndex = figureIndex;

            profitCombobox.ItemsSource = figureValues;
            profitCombobox.SelectedIndex = profitFigureIndex;

            statisticsDatePicker.SelectedDate = selectedDate;
            chartTabControl.SelectedIndex = tabSelectedIndex;

            configureGeneral();
            configureRevenueCharts();

            _specificStatistics = new SpecificStatistics(this);
            _advancedStatistics = new AdvancedStatistics(this);

            _specificStatistics.getAdvancedStatistic(_advancedStatistics);
            _advancedStatistics.getSpecificStatistic(_specificStatistics);

            DataContext = this;
        }

        private StatisticsBUS _statisticsBUS;
        public SpecificStatistics _specificStatistics;
        public AdvancedStatistics _advancedStatistics;
        public List<string> figureValues = new List<string>() { "Daily", "Weekly", "Monthly", "Yearly" };
        public List<string> statisticsFigureValues = new List<string>() { "General", "Specific", "Advanced" };
        public int statisticsFigureIndex { get; set; } = 0;
        public int figureIndex { get; set; } = 0;
        public int profitFigureIndex { get; set; } = 0;
        public int tabSelectedIndex { get; set; } = 0;
        public DateTime selectedDate { get; set; } = DateTime.Now;
        public System.Globalization.CultureInfo info = System.Globalization.CultureInfo.GetCultureInfo("vi-VN");

        public void configureGeneral()
        {
            TotalRevenueTextBlock.Text = _statisticsBUS.getTotalRevenueUntilDate(selectedDate).ToString();
            TotalProfitTextBlock.Text = _statisticsBUS.getTotalProfitUntilDate(selectedDate).ToString();
            TotalOrdersTextBlock.Text = _statisticsBUS.getTotalOrdersUntilDate(selectedDate).ToString();
        }

        public void configureRevenueCharts()
        {
            switch (figureIndex)
            {
                case 0:
                    var revenueResult = _statisticsBUS.getDailyRevenue(selectedDate);

                    var revenues = new ChartValues<double>();
                    var dates = new List<string>();

                    foreach (var item in revenueResult)
                    {
                        revenues.Add((double)item.Item2);
                        dates.Add(item.Item1.ToString());
                    }

                    var revenueCollection = new SeriesCollection()
                    {
                    new LineSeries
                    {
                        Title = "Revenue: ",
                        Values = revenues,
                        LabelPoint = point => String.Format(info, "{0:c}", point.Y)
                    }
                    };


                    revenueChart.AxisX.Clear();
                    revenueChart.AxisX.Add(new LiveCharts.Wpf.Axis
                    {
                        Title = "Date",
                        Labels = dates
                    });

                    revenueChart.AxisY.Clear();
                    revenueChart.AxisY.Add(new LiveCharts.Wpf.Axis
                    {
                        Title = "Revenue",
                        LabelFormatter = x => String.Format(info, "{0:c}", x)
                    });

                    revenueChart.Series = revenueCollection;
                    break;

                case 1:
                    var weeklyRevenueResult = _statisticsBUS.getWeeklyRevenue(selectedDate);

                    var weeklyRevenues = new ChartValues<double>();
                    var weeks = new List<string>();

                    foreach (var item in weeklyRevenueResult)
                    {
                        weeks.Add(item.Item1.ToString());
                        weeklyRevenues.Add((double)item.Item2);
                    }

                    var weeklyRevenueCollection = new SeriesCollection()
                    {
                    new ColumnSeries
                    {
                        Title = "Revenue: ",
                        Values = weeklyRevenues,
                        LabelPoint = point => String.Format(info, "{0:c}", point.Y)
                    }
                    };

                    revenueChart.AxisX.Clear();
                    revenueChart.AxisX.Add(new LiveCharts.Wpf.Axis
                    {
                        Title = "Week",
                        Labels = weeks
                    });

                    revenueChart.AxisY.Clear();
                    revenueChart.AxisY.Add(new LiveCharts.Wpf.Axis
                    {
                        Title = "Revenue",
                        LabelFormatter = x => String.Format(info, "{0:c}", x)
                    });

                    revenueChart.Series = weeklyRevenueCollection;
                    break;

                case 2:
                    var monthlyRevenueResult = _statisticsBUS.getMonthlyRevenue(selectedDate);

                    var monthlyRevenues = new ChartValues<double>();
                    var months = new List<string>();

                    foreach (var item in monthlyRevenueResult)
                    {
                        months.Add(item.Item1.ToString());
                        monthlyRevenues.Add((double)item.Item2);
                    }

                    var monthlyRevenueCollection = new SeriesCollection()
                    {
                    new ColumnSeries
                    {
                        Title = "Revenue: ",
                        Values = monthlyRevenues,
                        LabelPoint = point => String.Format(info, "{0:c}", point.Y)
                    }
                    };

                    revenueChart.AxisX.Clear();
                    revenueChart.AxisX.Add(new LiveCharts.Wpf.Axis
                    {
                        Title = "Month",
                        Labels = months
                    });

                    revenueChart.AxisY.Clear();
                    revenueChart.AxisY.Add(new LiveCharts.Wpf.Axis
                    {
                        Title = "Revenue",
                        LabelFormatter = x => String.Format(info, "{0:c}", x)
                    });

                    revenueChart.Series = monthlyRevenueCollection;

                    break;
                case 3:
                    var yearlyRevenueResult = _statisticsBUS.getYearlyRevenue();

                    var yearlyRevenues = new ChartValues<double>();
                    var years = new List<string>();

                    foreach (var item in yearlyRevenueResult)
                    {
                        years.Add(item.Item1.ToString());
                        yearlyRevenues.Add((double)item.Item2);
                    }

                    var yearlyRevenueCollection = new SeriesCollection()
                    {
                    new ColumnSeries
                    {
                        Title = "Revenue: ",
                        Values = yearlyRevenues,
                        LabelPoint = point => String.Format(info, "{0:c}", point.Y)
                    }
                    };


                    revenueChart.AxisX.Clear();
                    revenueChart.AxisX.Add(new LiveCharts.Wpf.Axis
                    {
                        Title = "Year",
                        Labels = years
                    });

                    revenueChart.AxisY.Clear();
                    revenueChart.AxisY.Add(new LiveCharts.Wpf.Axis
                    {
                        Title = "Revenue",
                        LabelFormatter = x => String.Format(info, "{0:c}", x)
                    });

                    revenueChart.Series = yearlyRevenueCollection;

                    break;
            }
        }

        public void configureProfitCharts()
        {
            switch (profitFigureIndex)
            {
                case 0:
                    var profitResult = _statisticsBUS.getDailyProfit(selectedDate);

                    var profits = new ChartValues<double>();
                    var dates = new List<string>();

                    foreach (var item in profitResult)
                    {
                        profits.Add((double)item.Item2);
                        dates.Add(item.Item1.ToString());
                    }

                    var profitCollection = new SeriesCollection()
                    {
                    new LineSeries
                    {
                        Title = "Profit: ",
                        Values = profits,
                        LabelPoint = point => String.Format(info, "{0:c}", point.Y)
                    }
                    };


                    profitChart.AxisX.Clear();
                    profitChart.AxisX.Add(new LiveCharts.Wpf.Axis
                    {
                        Title = "Date",
                        Labels = dates
                    });

                    profitChart.AxisY.Clear();
                    profitChart.AxisY.Add(new LiveCharts.Wpf.Axis
                    {
                        Title = "Profit",
                        LabelFormatter = x => String.Format(info, "{0:c}", x)
                    });

                    profitChart.Series = profitCollection;
                    break;

                case 1:
                    var weeklyProfitResult = _statisticsBUS.getWeeklyProfit(selectedDate);

                    var weeklyProfits = new ChartValues<double>();
                    var weeks = new List<string>();

                    foreach (var item in weeklyProfitResult)
                    {
                        weeks.Add(item.Item1.ToString());
                        weeklyProfits.Add((double)item.Item2);
                    }

                    var weeklyProfitCollection = new SeriesCollection()
                    {
                    new ColumnSeries
                    {
                        Title = "Profit: ",
                        Values = weeklyProfits,
                        LabelPoint = point => String.Format(info, "{0:c}", point.Y)
                    }
                    };


                    profitChart.AxisX.Clear();
                    profitChart.AxisX.Add(new LiveCharts.Wpf.Axis
                    {
                        Title = "Week",
                        Labels = weeks
                    });

                    profitChart.AxisY.Clear();
                    profitChart.AxisY.Add(new LiveCharts.Wpf.Axis
                    {
                        Title = "Profit",
                        LabelFormatter = x => String.Format(info, "{0:c}", x)
                    });

                    profitChart.Series = weeklyProfitCollection;

                    break;

                case 2:
                    var monthlyProfitResult = _statisticsBUS.getMonthlyProfit(selectedDate);

                    var monthlyProfits = new ChartValues<double>();
                    var months = new List<string>();

                    foreach (var item in monthlyProfitResult)
                    {
                        months.Add(item.Item1.ToString());
                        monthlyProfits.Add((double)item.Item2);
                    }

                    var monthlyProfitCollection = new SeriesCollection()
                    {
                    new ColumnSeries
                    {
                        Title = "Profit: ",
                        Values = monthlyProfits,
                        LabelPoint = point => String.Format(info, "{0:c}", point.Y)
                    }
                    };


                    profitChart.AxisX.Clear();
                    profitChart.AxisX.Add(new LiveCharts.Wpf.Axis
                    {
                        Title = "Month",
                        Labels = months
                    });

                    profitChart.AxisY.Clear();
                    profitChart.AxisY.Add(new LiveCharts.Wpf.Axis
                    {
                        Title = "Profit",
                        LabelFormatter = x => String.Format(info, "{0:c}", x)
                    });

                    profitChart.Series = monthlyProfitCollection;

                    break;
                case 3:
                    var yearlyProfitResult = _statisticsBUS.getYearlyProfit();

                    var yearlyProfits = new ChartValues<double>();
                    var years = new List<string>();

                    foreach (var item in yearlyProfitResult)
                    {
                        years.Add(item.Item1.ToString());
                        yearlyProfits.Add((double)item.Item2);
                    }

                    var yearlyProfitCollection = new SeriesCollection()
                    {
                    new ColumnSeries
                    {
                        Title = "Profit: ",
                        Values = yearlyProfits,
                        LabelPoint = point => String.Format(info, "{0:c}", point.Y)
                    }
                    };


                    profitChart.AxisX.Clear();
                    profitChart.AxisX.Add(new LiveCharts.Wpf.Axis
                    {
                        Title = "Year",
                        Labels = years
                    });

                    profitChart.AxisY.Clear();
                    profitChart.AxisY.Add(new LiveCharts.Wpf.Axis
                    {
                        Title = "Profit",
                        LabelFormatter = x => String.Format(info, "{0:c}", x)
                    });

                    profitChart.Series = yearlyProfitCollection;

                    break;
            }
        }

        private void revenueCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (tabSelectedIndex)
            {
                case 0:
                    configureGeneral();
                    configureRevenueCharts();
                    break;
                case 1:
                    configureGeneral();
                    configureProfitCharts();
                    break;
            }
        }

        private void statisticsDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (tabSelectedIndex)
            {
                case 0:
                    configureGeneral();
                    configureRevenueCharts();
                    break;
                case 1:
                    configureGeneral();
                    configureProfitCharts();
                    break;
            }
        }

        private void chartTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (tabSelectedIndex)
            {
                case 0:
                    configureGeneral();
                    configureRevenueCharts();
                    break;
                case 1:
                    configureGeneral();
                    configureProfitCharts();
                    break;
            }
        }

        private void profitCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (tabSelectedIndex)
            {
                case 0:
                    configureGeneral();
                    configureRevenueCharts();
                    break;
                case 1:
                    configureGeneral();
                    configureProfitCharts();
                    break;
            }
        }

        private void statisticsCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (statisticsFigureIndex)
            {
                case 0:
                    break;
                case 1:
                    NavigationService.Navigate(_specificStatistics);
                    statisticsFigureIndex = 0;
                    statisticsCombobox.SelectedIndex = statisticsFigureIndex;
                    break;
                case 2:
                    NavigationService.Navigate(_advancedStatistics);
                    statisticsFigureIndex = 0;
                    statisticsCombobox.SelectedIndex = statisticsFigureIndex;
                    break;
                default:
                    break;
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            AppConfig.SetValue(AppConfig.LastWindow, "Statistics");
        }
    }
}
