using Microsoft.Win32;
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
    /// Interaction logic for EditPhoneScreen.xaml
    /// </summary>
    public partial class EditPhoneScreen : Window
    {
        public Product EditedPhone { get; set; }
        public EditPhoneScreen(Product p)
        {
            InitializeComponent();
            EditedPhone = (Product)p.Clone();
            this.DataContext = EditedPhone;
        }

        private void editButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
        public static readonly List<string> ImageExtensions = new List<string> { ".JPG", ".JPE", ".BMP", ".GIF", ".PNG" };

        private void ImageButton_Click(object sender, RoutedEventArgs e)
        {
            var screen = new OpenFileDialog();
            screen.Filter = "Image Files|*.jpg;*.jpeg;*.png;...";
            if (screen.ShowDialog() == true)
            {
                EditedPhone.Avatar = new BitmapImage(new Uri(screen.FileName, UriKind.Absolute));
                avatar.Source = EditedPhone.Avatar;
            }
        }
    }
}
