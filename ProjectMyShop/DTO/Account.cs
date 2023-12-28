using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectMyShop.DTO
{
    public class Account: INotifyPropertyChanged
    {
        public string Username { get; set; }
        public string Rolename { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
