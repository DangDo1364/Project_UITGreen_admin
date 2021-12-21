using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project_UITGreen_admin.Models
{
    public class Order_user_items
    {
        public int id_user_items { set; get; }
        public int id_ord { set; get; }
        public int id_pro { set; get; }
        public int quantity { set; get; }
        public double price { set; get; }

        public static List<Order_user_items> SelectByID(int id)
        {
            List<Order_user_items> ord = new List<Order_user_items>();
            using (var context = new DataContext())
            {
                ord = context.Order_user_items.Where(p => p.id_ord == id).ToList();
            }
            return ord;
        }
    }
}
