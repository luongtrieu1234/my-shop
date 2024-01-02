using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ProjectMyShop.DTO
{
    public class Category: ICloneable
    {
        public int ID { get; set; }
        public string? CatName { get; set; }
        public BitmapImage Avatar { get; set; }
        public BindingList<Product>? Phones { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
