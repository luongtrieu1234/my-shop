using ProjectMyShop.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectMyShop.ViewModels
{
    internal class OrderViewModel : INotifyPropertyChanged
    {
        public BindingList<Order> Orders { get; set; } = new BindingList<Order>();
        public List<Order> SelectedOrders { get; set; } = new List<Order>();

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
