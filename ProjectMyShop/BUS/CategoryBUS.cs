using ProjectMyShop.DAO;
using ProjectMyShop.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectMyShop.BUS
{
    internal class CategoryBUS
    {

        private CategoryDAO _categoryDAO;

        public CategoryBUS()
        {
            _categoryDAO = new CategoryDAO();
            if (_categoryDAO.CanConnect())
            {
                _categoryDAO.Connect();
            }
        }

        public Category GetCategoryById(int id)
        {
            Category result = _categoryDAO.GetCategoryById(id);

            return result;
        }

        public List<Category> getCategoryList()
        {
            return _categoryDAO.getCategoryList();
        }
        public void AddCategory(Category cat)
        {
            int ID = _categoryDAO.isExisted(cat);
            if (ID > 0)
            {
                // existed category
                cat.ID = ID;
            }
            else
            {
                // add new category
                _categoryDAO.AddCategory(cat);
                cat.ID = _categoryDAO.GetLastestInsertID();
            }
        }

        public void removeCategory(Category cat)
        {
            _categoryDAO.removeCategory(cat.ID);
        }

        public void updateCategory(int ID, Category category)
        {

                _categoryDAO.updateCategory(ID, category);
            
        }
    }
}
