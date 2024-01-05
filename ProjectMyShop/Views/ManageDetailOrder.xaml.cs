using ProjectMyShop.BUS;
using ProjectMyShop.DTO;
using ProjectMyShop.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace ProjectMyShop.Views
{
    /// <summary>
    /// Interaction logic for ManageDetailOrder.xaml
    /// </summary>
    public partial class ManageDetailOrder : Window
    {
        public Order order;
        private OrderBUS _orderBUS;

        DetailOrderViewModel _vm;

        DetailOrder detailOrder;

        public ManageDetailOrder(Order order)
        {
            InitializeComponent();

            this.order = order;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (order.OrderDate.Equals(DateOnly.Parse(DateTime.MinValue.Date.ToShortDateString()))) 
                order.OrderDate = DateOnly.Parse(DateTime.Now.Date.ToShortDateString());
            OrderDatePicker.SelectedDate = DateTime.Parse(order.OrderDate.ToString());
            StatusComboBox.ItemsSource = Order.GetAllStatusValues();
            DataContext = order;

            if (order.ID == 0)
            {
                ChooseProductButton.IsEnabled = false;
                UpdateButton.IsEnabled = false;
                DeleteButton.IsEnabled = false;
            }

            _vm = new DetailOrderViewModel();
            if (order.DetailOrderList != null)
            {
                _vm.detailOrders = new BindingList<DetailOrder>(order.DetailOrderList);
                ProductDataGrid.ItemsSource = order.DetailOrderList;
            }

            _orderBUS = new OrderBUS();
            detailOrder = new DetailOrder();
            detailOrder.OrderID = order.ID;
        }

        private void ReturnButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // date only cannot bound so have to do this 
            if (OrderDatePicker.SelectedDate != null)
                order.OrderDate = DateOnly.Parse(OrderDatePicker.SelectedDate.Value.Date.ToShortDateString());
            DialogResult = true;
        }

        private DateOnly ConvertDateTimeToDateOnly(DateTime dateTime)
        {
            return DateOnly.Parse(dateTime.Date.ToShortDateString());
        }

        bool isInProductList(Product Product)
        {
            bool result = false;
            if (order.DetailOrderList != null)
            {
                foreach (DetailOrder detail in order.DetailOrderList) {
                    if (detail.Product.ID == Product.ID)
                    {
                        result = true;
                        break;
                    }
                    else
                    {
                        // do nothing
                    }
                }

            }

            return result;
        }

        private void ChooseProductButton_Click(object sender, RoutedEventArgs e)
        {
            detailOrder.Product = new Product();
            detailOrder.Product.ProductName = "Choose a Product";
            detailOrder.Quantity = 0;
            var screen = new AddProductOrder(detailOrder);
            if (screen.ShowDialog() == true)
            {
                if (order.DetailOrderList == null)
                    order.DetailOrderList = new List<DetailOrder>();
                if (!isInProductList(screen.detailOrder.Product))
                {
                    _orderBUS.AddDetailOrder(screen.detailOrder);
                    order.DetailOrderList.Add(screen.detailOrder);
                }
                else
                {
                    MessageBox.Show($"{screen.detailOrder.Product.ProductName}'s already exists in detail order.\nChoose 'Update' instead", "Duplicate Product", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                Reload();
            }
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            int i = ProductDataGrid.SelectedIndex;

            if (i != -1)
            {

                detailOrder.Product = new Product();
                detailOrder.Product = (Product)order.DetailOrderList[i].Product.Clone();
                detailOrder.Quantity = order.DetailOrderList[i].Quantity;
                var screen = new AddProductOrder(detailOrder);
                if (screen.ShowDialog() == true)
                {
                    if (order.DetailOrderList == null)
                        order.DetailOrderList = new List<DetailOrder>();
                    if (order.DetailOrderList[i].Product.ID == screen.detailOrder.Product.ID || !isInProductList(screen.detailOrder.Product))
                    {
                        _orderBUS.UpdateDetailOrder(order.DetailOrderList[i].Product.ID, screen.detailOrder);
                        order.DetailOrderList[i] = (DetailOrder)screen.detailOrder.Clone();
                    }
                    else
                    {
                        MessageBox.Show($"{screen.detailOrder.Product.ProductName}'s already exists in detail order.\nPlease choose another Product", "Duplicate Product", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    Reload();
                }
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            int i = ProductDataGrid.SelectedIndex;

            if (i != -1)
            {
                var res = MessageBox.Show($"Are you sure to discard this Product: {order.DetailOrderList[i].Product.ProductName}?", "Delete Product from order", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (res == MessageBoxResult.Yes)
                {
                    _orderBUS.DeleteDetailOrder(order.DetailOrderList[i]);
                    order.DetailOrderList.RemoveAt(i);
                    Reload();
                }
            }
            else
            {
                // do nothing
            }
        }

        void Reload()
        {

            if (order.DetailOrderList != null)
            {
                _vm.detailOrders = new BindingList<DetailOrder>(order.DetailOrderList);
                ProductDataGrid.ItemsSource = _vm.detailOrders;
            }
        }


        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

            int i = ProductDataGrid.SelectedIndex;
            if (i != -1)
            {
                var QuantityTextBox = e.OriginalSource as TextBox;

                if (QuantityTextBox != null)
                {
                    if (QuantityTextBox.Text == "")
                    {
                        QuantityTextBox.Text = "0";
                    }
                    else if ((order.DetailOrderList !=  null && (int.Parse(QuantityTextBox.Text)
                        > order.DetailOrderList[i].Product.Stock)))
                    {
                        QuantityTextBox.Text = QuantityTextBox.Text.Remove(QuantityTextBox.Text.Length - 1);

                        if ((order.DetailOrderList != null && (int.Parse(QuantityTextBox.Text)
                            > order.DetailOrderList[i].Product.Stock)))
                            QuantityTextBox.Text = order.DetailOrderList[i].Product.Stock.ToString();
                    }
                }

            }
        }

        private new void PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = IsTextAllowed(e.Text);
        }

        private bool IsTextAllowed(string text)
        {
            Regex _regex = new Regex("[^0-9]+$"); //regex that matches disallowed text
            return _regex.IsMatch(text);
        }

    }
}
