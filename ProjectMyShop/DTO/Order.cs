using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectMyShop.DTO
{
    public class Order
    {

        public enum OrderStatusEnum
        {
            Open = 0,
            Close = 1,
            Progess = 2
        }

        public static List<string> GetAllStatusValues()
        {
            var values = new List<string>();
            foreach (OrderStatusEnum status in Enum.GetValues(typeof(OrderStatusEnum)))
                values.Add(status.ToString());
            return values;
        }

        public int ID { get; set; }
        public string CustomerName { get; set; }
        public DateOnly OrderDate { get; set; }
        public OrderStatusEnum Status { get; set; }
        public string Address { get; set; }
        public Voucher? Voucher { get; set; }
        public List<DetailOrder> DetailOrderList { get; set; }
    }

}
