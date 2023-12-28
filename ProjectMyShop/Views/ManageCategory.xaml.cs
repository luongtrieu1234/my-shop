using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Aspose.Cells;
using Microsoft.Win32;
using ProjectMyShop.BUS;
using ProjectMyShop.DTO;
using ProjectMyShop.ViewModels;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ProjectMyShop.Config;
using System.Configuration;

namespace ProjectMyShop.Views
{
    /// <summary>
    /// Interaction logic for ManageCategory.xaml
    /// </summary>
    public partial class ManageCategory : Page
    {

        List<Category>? _categories = null;        
        CategoryViewModel CategoryViewModel = new CategoryViewModel();
        private CategoryBUS _categoryBUS = new CategoryBUS();
        

        public ManageCategory()
        {


            InitializeComponent();
            CategoryBUS catBUS = new CategoryBUS();
            CategoryViewModel.Categories = new BindingList<Category>(catBUS.getCategoryList());


        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var screen = new AddCategoryScreen();
            var result = screen.ShowDialog();
            if (result == true)
            {
                var newCategory = screen.newCategory;
                Debug.WriteLine(newCategory.CatName);
               

                try
                {
                    _categoryBUS.AddCategory(newCategory);           
                    loadCategory();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(screen, ex.Message);
                }

            }
            
        }

        void loadCategory()
        {
            _categories = _categoryBUS.getCategoryList();
            categoriesListView.ItemsSource = _categories;

        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var p = (Category)categoriesListView.SelectedItem;
            var screen = new EditCategoryScreen(p);
            var result = screen.ShowDialog();
            if (result == true)
            {
                var info = screen.EditedCategory;
                p.CatName = info.CatName;
                p.Avatar = info.Avatar;
                try
                {
                    _categoryBUS.updateCategory(p.ID, p);
                    loadCategory();

                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Exception here");
                    MessageBox.Show(screen, ex.Message);
                }
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var p = (Category)categoriesListView.SelectedItem;
            var result = MessageBox.Show($"Bạn thật sự muốn xóa hãng điện thoại {p.CatName}?",
                "Xác nhận xóa", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (MessageBoxResult.Yes == result)
            {

                _categoryBUS.removeCategory(p);
                loadCategory();

            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

            AppConfig.SetValue(AppConfig.LastWindow, "ManageCategory");
            loadCategory();


        }

        private void nextButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void deleteMenuItemClick(object sender, RoutedEventArgs e)
        {

        }

        private void editMenuItemClick(object sender, RoutedEventArgs e)
        {

        }

        private void previousButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void categoriesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

 
    }
}
