using ProjectMyShop.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ProjectMyShop.ViewModels
{
    internal class CategoryViewModel : INotifyPropertyChanged
    {
        public BindingList<Category> Categories { get; set; } = new BindingList<Category>();
        public List<Category> SelectedCategories { get; set; } = new List<Category>();

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
