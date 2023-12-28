using ProjectMyShop.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectMyShop.ViewModels
{
    internal class PhoneViewModel : INotifyPropertyChanged
    {
        public BindingList<Phone> Phones { get; set; } = new BindingList<Phone>();
        public List<Phone> SelectedPhones { get; set; } = new List<Phone>();

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
