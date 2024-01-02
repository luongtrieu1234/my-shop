using ProjectMyShop.DTO;
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
using System.Windows.Shapes;

namespace ProjectMyShop.Views
{
    /// <summary>
    /// Interaction logic for AddStockScreen.xaml
    /// </summary>
    public partial class AddStockScreen : Window
    {
        public Product newPhone { get; set; }
        public AddStockScreen(Product p)
        {
            InitializeComponent();
            newPhone = (Product)p.Clone();
            Debug.WriteLine(newPhone.Description);
            this.DataContext = newPhone;
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
