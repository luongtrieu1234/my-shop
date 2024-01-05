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
        private ProductDAO _productDAO;

        public ProductBUS()
        {
            _productDAO = new ProductDAO();
            if (_productDAO.CanConnect())
            {
                _productDAO.Connect();
            }
        }

        public int GetTotalProduct()
        {
            return _productDAO.getTotalProduct();
        }
        public List<Product> Top5OutStock()
        {
            return _productDAO.GetTop5OutStock();
        }

        public List<Product> getProductsAccordingToSpecificCategory(int srcCategoryID)
        {
            return _productDAO.getProductsAccordingToSpecificCategory(srcCategoryID);
        }

        public void addProduct(Product product)
        {
            if (product.Stock < 0)
            {
                throw new Exception("Invalid stock");
            }
            else if (product.BoughtPrice < 0 || product.SoldPrice < 0)
            {
                throw new Exception("Invalid price");
            }
            else
            {
                product.UploadDate = DateTime.Now.Date;
                _productDAO.addProduct(product);
                product.ID = _productDAO.GetLastestInsertID();
            }
        }
        public void removeProduct(Product product)
        {
            _productDAO.deleteProduct(product.ID);
        }
        public void updateProduct(int ID, Product product)
        {
            Debug.WriteLine(product.Stock);
            if (product.Stock < 0)
            {
                throw new Exception("Invalid stock");
            }
            else if (product.BoughtPrice < 0 || product.SoldPrice < 0)
            {
                throw new Exception("Invalid price");
            }
            else
            {
                _productDAO.updateProduct(ID, product);
            }
        }

        public List<BestSellingProduct> getBestSellingProductsInWeek(DateTime src)
        {
            return _productDAO.getBestSellingProductsInWeek(src);
        }

        public List<BestSellingProduct> getBestSellingProductsInMonth(DateTime src)
        {
            return _productDAO.getBestSellingProductsInMonth(src);
        }

        public List<BestSellingProduct> getBestSellingProductsInYear(DateTime src)
        {
            return _productDAO.getBestSellingProductsInYear(src);
        }
        public Product? getProductByID(int ProductID)
        {
            return _productDAO.getProductByID(ProductID);
        }
    }
}
