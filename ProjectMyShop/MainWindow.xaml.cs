using ProjectMyShop.Config;
using ProjectMyShop.Views;
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

namespace ProjectMyShop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        Dashboard dashboard;
        ManageProduct manageProductPage;
        ManageOrder _manageOrderPage;
        Statistics statisticsPage;
        ManageCategory _manageCategory;
        Configuration _configPage;
        Login login;
        Button[] buttons;
        

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            login = new Login();
            this.Opacity = 0.3;
            login.Owner = this;
            if (login.ShowDialog() == true)
            {
                this.Opacity = 1;
                dashboard = new Dashboard();
                _manageOrderPage = new ManageOrder();
                _manageCategory = new ManageCategory();
                statisticsPage = new Statistics();
                manageProductPage = new ManageProduct();

                Button[] buttons1 = new Button[] { dashboardButton, categoriesButton, productButton, orderButton, statButton, configButton };
                buttons = buttons1;
                var a = AppConfig.GetValue(AppConfig.LastWindow);
                if (AppConfig.GetValue(AppConfig.LastWindow) == "0")
                {
                    changeButtonColor(dashboardButton);
                    pageNavigation.NavigationService.Navigate(dashboard);
                }
                else
                {
                    if (AppConfig.GetValue(AppConfig.LastWindow) == "Dashboard")
                    {
                        changeButtonColor(dashboardButton);
                        pageNavigation.NavigationService.Navigate(dashboard);
                    }
                    else if(AppConfig.GetValue(AppConfig.LastWindow) == "ManageCategory")
                    {
                        changeButtonColor(categoriesButton);
                        pageNavigation.NavigationService.Navigate(_manageCategory);
                    }
                    else if (AppConfig.GetValue(AppConfig.LastWindow) == "ManageOrder")
                    {
                        changeButtonColor(orderButton);
                        pageNavigation.NavigationService.Navigate(_manageOrderPage);
                    }
                    else if (AppConfig.GetValue(AppConfig.LastWindow) == "Statistics")
                    {
                        changeButtonColor(statButton);
                        pageNavigation.NavigationService.Navigate(statisticsPage);
                    }

                    else if (AppConfig.GetValue(AppConfig.LastWindow) == "ManageProduct")
                    {
                        changeButtonColor(productButton);
                        pageNavigation.NavigationService.Navigate(manageProductPage);
                    }
                }
            }
            else
            {
                // quit
                this.Close();
            }
        }
        private void changeButtonColor(Button b)
        {
            foreach (var button in buttons)
            {
                button.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F94F2F"));
            }
            b.Background = (Brush)Application.Current.Resources["MyRedGradient"];
        }

        private void dashboardButton_Click(object sender, RoutedEventArgs e)
        {
            changeButtonColor(dashboardButton);
            dashboard = new Dashboard();
            pageNavigation.NavigationService.Navigate(dashboard);
        }

        private void productButton_Click(object sender, RoutedEventArgs e)
        {
            changeButtonColor(productButton);
            manageProductPage = new ManageProduct();
            pageNavigation.NavigationService.Navigate(manageProductPage);
        }

        private void orderButton_Click(object sender, RoutedEventArgs e)
        {
            changeButtonColor(orderButton);
            pageNavigation.NavigationService.Navigate(_manageOrderPage);
        }

        private void statButton_Click(object sender, RoutedEventArgs e)
        {
            changeButtonColor(statButton);
            statisticsPage = new Statistics();
            pageNavigation.NavigationService.Navigate(statisticsPage);
        }

        private void configButton_Click(object sender, RoutedEventArgs e)
        {
            changeButtonColor(configButton);
            _configPage = new Configuration();
            pageNavigation.NavigationService.Navigate(_configPage);
        }

        private void categoriesButton_Click(object sender, RoutedEventArgs e)
        {
            changeButtonColor(categoriesButton);
            pageNavigation.NavigationService.Navigate(_manageCategory);
        }
    }
}
