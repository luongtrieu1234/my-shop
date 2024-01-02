using ProjectMyShop.BUS;
using ProjectMyShop.Config;
using ProjectMyShop.DTO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ProjectMyShop.Views
{
    /// <summary>
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Dashboard : Page
    {
        public int totalPhone { get; set; } = 0;
        public int weekOrder { get; set; } = 0;
        public int monthOrder { get; set; } = 0;

        List<Product>? _products = null;
        ProductBUS _productBUS = new ProductBUS();
        OrderBUS _orderBUS = new OrderBUS();
        public Dashboard()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            totalPhone = _productBUS.GetTotalPhone();
            weekOrder = _orderBUS.CountOrderByWeek();
            monthOrder = _orderBUS.CountOrderByMonth();
            _products = _productBUS.Top5OutStock();

            PhoneDataGrid.ItemsSource = _products;
            DataContext = this;
            AppConfig.SetValue(AppConfig.LastWindow, "Dashboard");
        }

        private void ProductDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void AddStockButton_Click(object sender, RoutedEventArgs e)
        {
            var row = GetParent<DataGridRow>((Button)sender);
            int index = PhoneDataGrid.Items.IndexOf(row.Item);
            if (index != -1)
            {
                Product p = _products![index];
                var screen = new AddStockScreen(p);
                var result = screen.ShowDialog();
                if (result == true)
                {
                    try
                    {
                        var newPhone = screen.newPhone;
                        _productBUS.updatePhone(p.ID, newPhone);

                        // reload page
                        _products = _productBUS.Top5OutStock();
                        PhoneDataGrid.ItemsSource = _products;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }
        private TargetType GetParent<TargetType>(DependencyObject o) where TargetType : DependencyObject
        {
            if (o == null || o is TargetType) return (TargetType)o;
            return GetParent<TargetType>(VisualTreeHelper.GetParent(o));
        }
    }
}
