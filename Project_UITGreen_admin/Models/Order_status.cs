using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project_UITGreen_admin.Models
{
    public class Order_status
    {
        public int id { set; get; }
        public int id_ord { set; get; }
        public string status { set; get; }
        public DateTime date { set; get; }
        public string address { set; get; }
        public static int Insert(int id_ord, string status, string address)
        {
            using (var context = new DataContext())
            {
                context.Order_status.Add(new Order_status
                {
                    id_ord = id_ord,
                    status = status,
                    date = DateTime.Now,
                    address = address
                });
                return context.SaveChanges();
            }
        }
        public static Order_status SelectOrdByID(int id)
        {
            Order_status ord = new Order_status();
            using (var context = new DataContext())
            {
                ord = context.Order_status.Where(p => p.id_ord == id && p.status== "Đã đặt hàng").FirstOrDefault();
            }
            return ord;
        }
    }
}
