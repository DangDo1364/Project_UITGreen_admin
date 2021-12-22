using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
namespace Project_UITGreen_admin.Models
{
    public class Orders
    {
        public int id_ord { set; get; }
        public int id_customer { set; get; }
        public int id_promotion { set; get; }
        public double ship { set; get; }
        public int paymethod { set; get; }
        public int status { set; get; }
        public string note { set; get; }
        public DateTime date { set; get; }
        public double price_sum { set; get; }
        // Xử lý
        public static List<Orders> SelectOrd(int status)
        {
            List<Orders> listOrd = new List<Orders>();
            using (var context = new DataContext())
            {
                listOrd = context.Orders.Where(p => p.status==status).OrderByDescending(p=>p.date).ToList();
            }
            return listOrd;
        }
        public static List<Orders> Select()
        {
            List<Orders> listOrd = new List<Orders>();
            using (var context = new DataContext())
            {
                listOrd = context.Orders.ToList();
            }
            return listOrd;
        }
        public static Orders SelectOrdByID(int id)
        {
            Orders ord = new Orders();
            using (var context = new DataContext())
            {
                ord = context.Orders.Where(p => p.id_ord==id).FirstOrDefault();
            }
            return ord;
        }
        public static List<Orders> SelectDate(DateTime date)
        {
            string date1 = date.ToString("dd-MM-yyyy");
            List<Orders> listOrd = new List<Orders>();
            using (var context = new DataContext())
            {
                listOrd = context.Orders.Where(p => p.date.ToString("dd-MM-yyyy").Contains(date1) && p.status > 0 && p.status < 4).ToList();
            }
            return listOrd;

        }
        public static void UpdateStatus(int id, int status)
        {
            using (var context = new DataContext())
            {
                var ord = context.Orders;
                Orders orders = (from p in ord
                                    where (p.id_ord == id)
                                    select p).FirstOrDefault();
                if (orders != null)
                {
                    orders.status = status;
                }
                context.SaveChanges();

            }
        }        
    }
}
