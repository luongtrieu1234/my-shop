using Microsoft.Win32;
using ProjectMyShop.DTO;
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
    /// Interaction logic for EditCategoryScreen.xaml
    /// </summary>
    public partial class EditCategoryScreen : Window
    {
        public EditCategoryScreen(Category p)
        {
            InitializeComponent();
            EditedCategory = (Category)p.Clone();
            this.DataContext = EditedCategory;
        }

        public Category EditedCategory { get; set; }


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
                EditedCategory.Avatar = new BitmapImage(new Uri(screen.FileName, UriKind.Absolute));
                avatar.Source = EditedCategory.Avatar;
            }
        }
    }
}
