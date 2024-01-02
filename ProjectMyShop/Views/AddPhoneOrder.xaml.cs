using ProjectMyShop.BUS;
using ProjectMyShop.DTO;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace ProjectMyShop.Views
{
    /// <summary>
    /// Interaction logic for AddProductOrder.xaml
    /// </summary>
    public partial class AddProductOrder : Window
    {
        ProductBUS _phoneBus;
        CategoryBUS _categoryBus;
        List<Category> _categories;
        List<Product> _selectedPhones;
        public DetailOrder detailOrder;


        public AddProductOrder(DetailOrder detailOrder)
        {
            InitializeComponent();
            this.detailOrder = (DetailOrder)detailOrder.Clone();
        }


        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            int i = PhoneListView.SelectedIndex;
            if (i != -1)
            {
                detailOrder.Product = _selectedPhones[i];
            }
            detailOrder.Quantity = int.Parse(QuantityTextBox.Text);
            DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _phoneBus = new ProductBUS();
            _categoryBus = new CategoryBUS();

            _categories = _categoryBus.getCategoryList();

            categoryCombobox.ItemsSource = _categories;

            categoryCombobox.SelectedIndex = 0;

            if (categoryCombobox.SelectedIndex >= 0)
            {
                _selectedPhones = _phoneBus.getProductsAccordingToSpecificCategory(_categories[categoryCombobox.SelectedIndex].ID);
                PhoneListView.ItemsSource = _selectedPhones;
            }

            DataContext = detailOrder;
        }

        private void categoryCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (categoryCombobox.SelectedIndex >= 0)
            {
                _selectedPhones = _phoneBus.getProductsAccordingToSpecificCategory(_categories[categoryCombobox.SelectedIndex].ID);
                PhoneListView.ItemsSource = _selectedPhones;
            }
        }


        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

            var QuantityTextBox = e.OriginalSource as TextBox;

            if (QuantityTextBox != null)
            {
                if (QuantityTextBox.Text == "")
                {
                    QuantityTextBox.Text = "0";
                }
                else if ((int.Parse(QuantityTextBox.Text)
                    > detailOrder.Product.Stock))
                {
                    QuantityTextBox.Text = QuantityTextBox.Text.Remove(QuantityTextBox.Text.Length - 1);

                    if (int.Parse(QuantityTextBox.Text)
                        > detailOrder.Product.Stock)
                        QuantityTextBox.Text = detailOrder.Product.Stock.ToString();
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

        private void PhoneListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int i = PhoneListView.SelectedIndex;
            if (i != -1)
            {
                detailOrder.Product = _selectedPhones[(int)i];
                detailOrder.Quantity = 0;

                PhoneTextBox.Text = detailOrder.Product.ProductName;
                QuantityTextBox.Text = detailOrder.Quantity.ToString();
            }
        }
    }
}
