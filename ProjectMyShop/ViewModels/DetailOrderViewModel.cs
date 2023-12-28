using ProjectMyShop.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectMyShop.ViewModels
{
    internal class DetailOrderViewModel : INotifyPropertyChanged
    {
        public BindingList<DetailOrder> detailOrders { get; set; } = new BindingList<DetailOrder>();

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
