using ProjectMyShop.Config;
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

namespace ProjectMyShop.Views
{
    /// <summary>
    /// Interaction logic for Configuration.xaml
    /// </summary>
    public partial class Configuration : Page
    {

        private string nProduct = "10";

        public Configuration()
        {
            InitializeComponent();
        }

        private void nProductComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            var item = (ComboBoxItem)nProductComboBox.SelectedValue;

            var content = "";

            if(item != null)
            {
                content = (string)item.Content;
            }
            
            if(content != "")
                AppConfig.SetValue(AppConfig.NumberProductPerPage, content);

            if (lastWindowCheckBox.IsChecked == false)
                AppConfig.SetValue(AppConfig.OpenLastWindow, "0");
            else
                AppConfig.SetValue(AppConfig.OpenLastWindow, "1");
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (AppConfig.GetValue(AppConfig.NumberProductPerPage) != null)
            {
                nProduct = AppConfig.GetValue(AppConfig.NumberProductPerPage);
            }

            if (nProduct == "3")
                nProductComboBox.SelectedIndex = 0;
            else if (nProduct == "6")
                nProductComboBox.SelectedIndex = 1;
            else if (nProduct == "9")
                nProductComboBox.SelectedIndex = 2;
            else if (nProduct == "12")
                nProductComboBox.SelectedIndex = 3;
            else if (nProduct == "20")
                nProductComboBox.SelectedIndex = 4;

            if (AppConfig.GetValue(AppConfig.OpenLastWindow) == "0")
                lastWindowCheckBox.IsChecked = false;
            else 
                lastWindowCheckBox.IsChecked = true;
        }
    }
}
