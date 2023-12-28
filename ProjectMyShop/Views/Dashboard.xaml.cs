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

        List<Phone>? _phones = null;
        PhoneBUS _phoneBUS = new PhoneBUS();
        OrderBUS _orderBUS = new OrderBUS();
        public Dashboard()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            totalPhone = _phoneBUS.GetTotalPhone();
            weekOrder = _orderBUS.CountOrderByWeek();
            monthOrder = _orderBUS.CountOrderByMonth();
            _phones = _phoneBUS.Top5OutStock();

            PhoneDataGrid.ItemsSource = _phones;
            DataContext = this;
            AppConfig.SetValue(AppConfig.LastWindow, "Dashboard");
        }

        private void PhoneDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void AddStockButton_Click(object sender, RoutedEventArgs e)
        {
            var row = GetParent<DataGridRow>((Button)sender);
            int index = PhoneDataGrid.Items.IndexOf(row.Item);
            if (index != -1)
            {
                Phone p = _phones![index];
                var screen = new AddStockScreen(p);
                var result = screen.ShowDialog();
                if (result == true)
                {
                    try
                    {
                        var newPhone = screen.newPhone;
                        _phoneBUS.updatePhone(p.ID, newPhone);

                        // reload page
                        _phones = _phoneBUS.Top5OutStock();
                        PhoneDataGrid.ItemsSource = _phones;
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
