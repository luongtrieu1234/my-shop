using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ProjectMyShop.DTO
{
    public class Product: ICloneable
    {
        public int ID { get; set; }
        public string ProductName { get; set; } = "";
        public string Manufacturer { get; set; } = "";
        public int Stock { get; set; }
        public string Description { get; set; } = "";
        public int BoughtPrice { get; set; }
        public int SoldPrice { get; set; }
        public Category Category { get; set; }
        public DateTime UploadDate { get; set; }
        public BitmapImage Avatar { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }

    public class BestSellingProduct : Product
    {
        public int Quantity { get; set; }
    }
}
