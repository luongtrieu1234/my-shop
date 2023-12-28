using Microsoft.Win32;
using ProjectMyShop.DTO;
using ProjectMyShop.BUS;
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
using System.Windows.Shapes;

namespace ProjectMyShop.Views
{
    /// <summary>
    /// Interaction logic for AddPhoneScreen.xaml
    /// </summary>
    public partial class AddPhoneScreen : Window
    {
        public Phone newPhone { get; set; }
        public int catIndex { get; set; } = -1;
        PhoneBUS _phoneBUS { get; set; }
        

        public AddPhoneScreen(List<Category> category)
        {
            InitializeComponent();
            newPhone = new Phone();
            this.DataContext = newPhone;
            categoryCombobox.ItemsSource = category;
        }

        private void categoryCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            catIndex = categoryCombobox.SelectedIndex;
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            // Check validity

            //Phone phone = new Phone()
            //{
            //    PhoneName = "Galaxy",
            //    Manufacturer = "Samsung",
            //    BoughtPrice = 500,
            //    SoldPrice = 700,
            //    Description = "stronglymanfok@outlook.com"
            //};
            if(catIndex < 0)
            {
                MessageBox.Show(this, "Invalid category");
            }
            else
            {
                DialogResult = true;
            }
            

        }

        private void chooseImageButton_Click(object sender, RoutedEventArgs e)
        {
            var screen = new OpenFileDialog();
            if(screen.ShowDialog() == true)
            {
                newPhone.Avatar = new BitmapImage(new Uri(screen.FileName, UriKind.Absolute));
                avatar.Source = newPhone.Avatar;
            }
        }
    }
}
