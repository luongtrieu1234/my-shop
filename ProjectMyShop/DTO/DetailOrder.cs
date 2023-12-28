using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectMyShop.DTO
{
    public class DetailOrder : ICloneable
    {
        public int OrderID { get; set; }
        public Phone Phone { get; set; }
        public int Quantity { get; set; }

        public object Clone()
        {
            return new DetailOrder() {  OrderID = OrderID, Phone = (Phone)Phone.Clone(), Quantity = Quantity };
        }
    }
}
