using ProjectMyShop.DAO;
using ProjectMyShop.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectMyShop.BUS
{
    internal class ProductBUS
    {
        private ProductDAO _phoneDAO;

        public ProductBUS()
        {
            _phoneDAO = new ProductDAO();
            if (_phoneDAO.CanConnect())
            {
                _phoneDAO.Connect();
            }
        }

        public int GetTotalPhone()
        {
            return _phoneDAO.getTotalProduct();
        }
        public List<Product> Top5OutStock()
        {
            return _phoneDAO.GetTop5OutStock();
        }

        public List<Product> getPhonesAccordingToSpecificCategory(int srcCategoryID)
        {
            return _phoneDAO.getProductsAccordingToSpecificCategory(srcCategoryID);
        }

        public void addPhone(Product phone)
        {
            if (phone.Stock < 0)
            {
                throw new Exception("Invalid stock");
            }
            else if (phone.BoughtPrice < 0 || phone.SoldPrice < 0)
            {
                throw new Exception("Invalid price");
            }
            else
            {
                phone.UploadDate = DateTime.Now.Date;
                _phoneDAO.addPhone(phone);
                phone.ID = _phoneDAO.GetLastestInsertID();
            }
        }
        public void removePhone(Product phone)
        {
            _phoneDAO.deleteProduct(phone.ID);
        }
        public void updatePhone(int ID, Product phone)
        {
            Debug.WriteLine(phone.Stock);
            if (phone.Stock < 0)
            {
                throw new Exception("Invalid stock");
            }
            else if (phone.BoughtPrice < 0 || phone.SoldPrice < 0)
            {
                throw new Exception("Invalid price");
            }
            else
            {
                _phoneDAO.updateProduct(ID, phone);
            }
        }

        public List<BestSellingPhone> getBestSellingPhonesInWeek(DateTime src)
        {
            return _phoneDAO.getBestSellingProductsInWeek(src);
        }

        public List<BestSellingPhone> getBestSellingPhonesInMonth(DateTime src)
        {
            return _phoneDAO.getBestSellingProductsInMonth(src);
        }

        public List<BestSellingPhone> getBestSellingPhonesInYear(DateTime src)
        {
            return _phoneDAO.getBestSellingProductsInYear(src);
        }
        public Product? getPhoneByID(int phoneID)
        {
            return _phoneDAO.getProductByID(phoneID);
        }
    }
}
