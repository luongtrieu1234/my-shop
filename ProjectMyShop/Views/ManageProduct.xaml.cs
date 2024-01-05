using Aspose.Cells;
using Microsoft.Win32;
using ProjectMyShop.BUS;
using ProjectMyShop.Config;
using ProjectMyShop.DTO;
using ProjectMyShop.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProjectMyShop.Views
{
    /// <summary>
    /// Interaction logic for ManageProduct.xaml
    /// </summary>
    public partial class ManageProduct : Page
    {
        public ManageProduct()
        {
            InitializeComponent();
        }
        private ProductBUS _ProductBus = new ProductBUS();
        ProductViewModel _vm = new ProductViewModel();
        List<Category>? _categories = null;
        int _totalItems = 0;
        int _currentPage = 1;
        int _totalPages = 0;
        int _rowsPerPage = int.Parse(AppConfig.GetValue(AppConfig.NumberProductPerPage));
        int i = 0;

        private void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string search_text = searchTextBox.Text;
            if(search_text.Length > 0)
            {
                _currentPage = 1;
                previousButton.IsEnabled = false;

                _vm.SelectedProducts.Clear();
                BindingList<Product> Products = new BindingList<Product>();
                foreach (Product Product in _vm.Products)
                {
                    if(Product.ProductName.ToLower().Contains(search_text.ToLower()))
                    {
                        Products.Add(Product);
                    }
                }

                _vm.SelectedProducts = Products
                .Skip((_currentPage - 1) * _rowsPerPage)
                .Take(_rowsPerPage).ToList();

                if(_vm.SelectedProducts.Count > 0)
                {
                    _currentPage = 1;
                    _totalItems = Products.Count;
                    _totalPages = Products.Count / _rowsPerPage +
                    (Products.Count % _rowsPerPage == 0 ? 0 : 1);
                    ProductsListView.ItemsSource = _vm.SelectedProducts;
                    currentPagingTextBlock.Text = $"{_currentPage}/{_totalPages}";
                }
                if(_totalPages <= 1)
                {
                    nextButton.IsEnabled = false;
                }
            }
            else
            {
                loadProducts();
            }
        }
        

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            previousButton.IsEnabled = false;
            nextButton.IsEnabled = false;
            _currentPage = 0;
            _totalPages = 0;
            var catBUS = new CategoryBUS();
            var ProductBUS = new ProductBUS();
            _categories = catBUS.getCategoryList();
            categoriesListView.ItemsSource = _categories;
            foreach (var category in _categories)
            {
                category.Products = new BindingList<Product>(ProductBUS.getProductsAccordingToSpecificCategory(category.ID));
            }
            if(_categories.Count > 0)
            {
                loadProducts();
            }

            AppConfig.SetValue(AppConfig.LastWindow, "ManageProduct");
        }

        private void filterRangeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        void loadProducts()
        {
            i = categoriesListView.SelectedIndex;
            if(i < 0)
            {
                i = 0;
            }


            if(_categories == null)
            {
                return;
            }
            _currentPage = 1;
            previousButton.IsEnabled = false;
            _vm.Products = _categories[i].Products;
            _vm.SelectedProducts = _vm.Products
                .Skip((_currentPage - 1) * _rowsPerPage)
                .Take(_rowsPerPage).ToList();

            _totalItems = _vm.Products.Count;
            _totalPages = _vm.Products.Count / _rowsPerPage +
                (_vm.Products.Count % _rowsPerPage == 0 ? 0 : 1);
            _currentPage = _totalPages > 0 ? 1 : 0;
            currentPagingTextBlock.Text = $"{_currentPage}/{_totalPages}";

            if(_totalPages > 1)
            {
                nextButton.IsEnabled = true;
            }
            else
            {
                nextButton.IsEnabled=false;
            }

            ProductsListView.ItemsSource = _vm.SelectedProducts;
            
        }

        private void categoriesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            previousButton.IsEnabled = false;
            loadProducts();
        }

        private void editMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var p = (Product)ProductsListView.SelectedItem;
            var screen = new EditProductScreen(p);
            var result = screen.ShowDialog();
            if (result == true)
            {
                var info = screen.EditedProduct;
                p.ProductName = info.ProductName;
                p.Manufacturer = info.Manufacturer;
                p.SoldPrice = info.SoldPrice;
                p.BoughtPrice = info.BoughtPrice;
                p.Description = info.Description;
                p.Avatar = info.Avatar;
                p.Stock = info.Stock;
                try
                {
                    _ProductBus.updateProduct(p.ID, p);
                    searchTextBox_TextChanged(sender, null);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Exception here");
                    MessageBox.Show(screen, ex.Message);
                }

                //_vm.Products = _categories[i].Products;
                //_vm.SelectedProducts = _vm.Products
                //    .Skip((_currentPage - 1) * _rowsPerPage)
                //    .Take(_rowsPerPage).ToList();

                //ProductsListView.ItemsSource = _vm.SelectedProducts;
            }
        }

        private void deleteMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var p = (Product)ProductsListView.SelectedItem;
            var result = MessageBox.Show($"Bạn thật sự muốn xóa điện thoại {p.ProductName} - {p.Manufacturer}?",
                "Xác nhận xóa", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (MessageBoxResult.Yes == result)
            {
                //_products.Remove(p);
                _vm.Products.Remove(p);
                _categories[i].Products.Remove(p);
                _ProductBus.removeProduct(p);
                searchTextBox_TextChanged(sender, null);
                //_vm.SelectedProducts.Remove(p);

                //_vm.SelectedProducts = _vm.Products
                //    .Skip((_currentPage - 1) * _rowsPerPage)
                //    .Take(_rowsPerPage).ToList();




                //// Tính toán lại thông số phân trang
                //_totalItems = _vm.Products.Count;
                //_totalPages = _vm.Products.Count / _rowsPerPage +
                //    (_vm.Products.Count % _rowsPerPage == 0 ? 0 : 1);

                //currentPagingTextBlock.Text = $"{_currentPage}/{_totalPages}";
                //if (_currentPage + 1 > _totalPages)
                //{
                //    nextButton.IsEnabled = false;
                //}

                //ProductsListView.ItemsSource = _vm.SelectedProducts;
            }
        }

        private void previousButton_Click(object sender, RoutedEventArgs e)
        {
            nextButton.IsEnabled = true;
            _currentPage--;
            _vm.SelectedProducts = _vm.Products
                .Skip((_currentPage - 1) * _rowsPerPage)
                .Take(_rowsPerPage)
                .ToList();

            // ép cập nhật giao diện
            ProductsListView.ItemsSource = _vm.SelectedProducts;
            currentPagingTextBlock.Text = $"{_currentPage}/{_totalPages}";
            if (_currentPage - 1 < 1)
            {
                previousButton.IsEnabled = false;
            }
        }

        private void nextButton_Click(object sender, RoutedEventArgs e)
        {
            previousButton.IsEnabled = true;
            _currentPage++;
            _vm.SelectedProducts = _vm.Products
                    .Skip((_currentPage - 1) * _rowsPerPage)
                    .Take(_rowsPerPage)
                    .ToList();

            // ép cập nhật giao diện
            ProductsListView.ItemsSource = _vm.SelectedProducts;
            currentPagingTextBlock.Text = $"{_currentPage}/{_totalPages}";

            if (_currentPage + 1 > _totalPages)
            {
                nextButton.IsEnabled = false;
            }
        }

        private void ImportButton_Click(object sender, RoutedEventArgs e)
        {
            _categories = new List<Category>();
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            var screen = new OpenFileDialog();

            if (screen.ShowDialog() == true)
            {
                string filename = screen.FileName;

                var workbook = new Workbook(filename);
                var _ProductBUS = new ProductBUS();
                var _cateBUS = new CategoryBUS();

                var tabs = workbook.Worksheets;
                // In ra các tab để d ebug
                foreach (var tab in tabs)
                {
                    Category cat = new Category()
                    {
                        CatName = tab.Name,
                        Products = new BindingList<Product>()
                    };
                    _cateBUS.AddCategory(cat);

                    // Bắt đầu từ ô B3
                    var column = 'C';
                    var row = 4;

                    var cell = tab.Cells[$"{column}{row}"];

                    while (cell.Value != null)
                    {
                        string name = cell.StringValue;
                        string manu = tab.Cells[$"D{row}"].StringValue;
                        int boughtprice = tab.Cells[$"E{row}"].IntValue;
                        int soldprice = tab.Cells[$"F{row}"].IntValue;
                        int stock = tab.Cells[$"G{row}"].IntValue;
                        string desc = tab.Cells[$"H{row}"].StringValue;
                        DateTime uploaddate = DateTime.Now;
                        if (tab.Cells[$"I{row}"].Type != CellValueType.IsNull)
                        {
                            uploaddate = tab.Cells[$"I{row}"].DateTimeValue;
                        }
                        string avaURL = tab.Cells[$"J{row}"].StringValue;

                        var p = new Product()
                        {
                            ProductName = name,
                            Manufacturer = manu,
                            BoughtPrice = boughtprice,
                            SoldPrice = soldprice,
                            Stock = stock,
                            Description = desc,
                            UploadDate = uploaddate,
                            Avatar = new BitmapImage(new Uri(avaURL, UriKind.Absolute)),
                            Category = cat,
                        };
                        _ProductBUS.addProduct(p);
                        row++;
                        cell = tab.Cells[$"{column}{row}"];
                    }
                }
                _categories = _cateBUS.getCategoryList();
                Debug.WriteLine(_categories.Count);
                foreach(var category in _categories)
                {
                    category.Products = new BindingList<Product>(_ProductBUS.getProductsAccordingToSpecificCategory(category.ID));
                }
                categoriesListView.ItemsSource = _categories;
                loadProducts();
            }
        }
        
        private void AddMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var screen = new AddProductScreen(_categories!);
            var result = screen.ShowDialog();
            if (result == true)
            {
                var newProduct = screen.newProduct;
                Debug.WriteLine(newProduct.ProductName);
                var catIndex = screen.catIndex;
                if(catIndex >= 0)
                {
                    try
                    {
                    newProduct.Category = _categories[catIndex];
                    _ProductBus.addProduct(newProduct);
                    _categories[catIndex].Products.Add(newProduct);
                    loadProducts();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(screen, ex.Message);
                    }
                }
            }
        }

        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            float fromPrice = float.Parse(fromTextbox.Text);
            float toPrice = float.Parse(toTextbox.Text);
            if (fromPrice >= 0 && toPrice > 0 && fromPrice < toPrice)
            {
                _currentPage = 1;
                previousButton.IsEnabled = false;

                _vm.SelectedProducts.Clear();
                BindingList<Product> Products = new BindingList<Product>();
                foreach (Product Product in _vm.Products)
                {
                    if (Product.SoldPrice >= fromPrice && Product.SoldPrice <= toPrice)
                    {
                        Products.Add(Product);
                    }
                }

                if(Products.Count <= 0)
                {
                    MessageBox.Show("Product not found!");
                    return;
                }

                _vm.SelectedProducts = Products
                .Skip((_currentPage - 1) * _rowsPerPage)
                .Take(_rowsPerPage).ToList();

                if (_vm.SelectedProducts.Count > 0)
                {
                    _currentPage = 1;
                    _totalItems = Products.Count;
                    _totalPages = Products.Count / _rowsPerPage +
                    (Products.Count % _rowsPerPage == 0 ? 0 : 1);
                    ProductsListView.ItemsSource = _vm.SelectedProducts;
                    currentPagingTextBlock.Text = $"{_currentPage}/{_totalPages}";
                }
                if (_totalPages <= 1)
                {
                    nextButton.IsEnabled = false;
                }
            }
            else
            {
               
                MessageBox.Show("Product not found!");
            }
        }
    }
}
