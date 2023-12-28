using ProjectMyShop.Config;
using ProjectMyShop.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectMyShop.BUS
{
    internal class AccountBUS
    {
        private AccountDAO _accountDAO;

        public AccountBUS()
        {
            _accountDAO = new AccountDAO();
        }

        public bool validate(string username, string password)
        {
            // save username & password to config file
            AppConfig.SetValue(AppConfig.Username, username);
            AppConfig.SetPassword(password);

            _accountDAO.ResetConnection();

            return _accountDAO.CanConnect();
        }
    }
}
