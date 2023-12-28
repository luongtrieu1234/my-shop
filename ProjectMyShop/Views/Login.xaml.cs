using ProjectMyShop.BUS;
using ProjectMyShop.Config;
using ProjectMyShop.DAO;
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
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {

        AccountBUS _accountBUS = new AccountBUS();

        private string _username;
        private string _password;

        public Login()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _username = "username";
            _password = "password";

            bool canGetAccount = false;
            try
            {
                _username = AppConfig.GetValue(key: AppConfig.Username);
                _password = AppConfig.GetPassword();
                canGetAccount = true;
            }
            catch { System.Diagnostics.Debug.WriteLine("First time running - Cannot read username/password"); }   

            if (canGetAccount)
            {
                TextBoxEmail.Text = _username;
                PasswordBox.Password = _password;
            }

        }

        private void ButtonLogin_Click(object sender, RoutedEventArgs e)
        {
            _username = TextBoxEmail.Text;
            _password = PasswordBox.Password;


            if (_username != null && _password != null)
            {
                
                // validate account
                if (_accountBUS.validate(_username, _password))
                {
                    DialogResult = true;
                }
                else
                {
                    MessageBox.Show("Wrong username or password", "Login", MessageBoxButton.OK, MessageBoxImage.Warning);
                    System.Diagnostics.Debug.WriteLine("Cannot connect to db");
                }
            }
            else
            {
                // gain focus 
            }

        }

    }
}
