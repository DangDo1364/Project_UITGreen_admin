using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project_UITGreen_admin.Models
{
    public class Orders_user
    {
        public int id_ord { set; get; }
        public int id_user { set; get; }
        public int id_promotion { set; get; }
        public double ship { set; get; }
        public int paymethod { set; get; }
        public int status { set; get; }
        public string note { set; get; }
        public DateTime date { set; get; }
        public double price_sum { set; get; }
        public static List<Orders_user> SelectOrd(int status)
        {
            List<Orders_user> listOrd = new List<Orders_user>();
            using (var context = new DataContext())
            {
                listOrd = context.Orders_user.Where(p => p.status == status).OrderByDescending(p=>p.date).ToList();
            }
            return listOrd;
        }
        public static List<Orders_user> SelectOrdTop()
        {
            List<Orders_user> listOrd = new List<Orders_user>();
            using (var context = new DataContext())
            {
                listOrd = context.Orders_user.Where(p => p.status > 0 && p.status < 4).ToList();
            }
            return listOrd;
        }
        public static List<Orders_user> SelectDate(DateTime date)
        {
            string date1 = date.ToString("dd-MM-yyyy");
            List<Orders_user> listOrd = new List<Orders_user>();
            using (var context = new DataContext())
            {
                listOrd = context.Orders_user.Where(p => p.date.ToString("dd-MM-yyyy").Contains(date1) && p.status > 0 && p.status < 4).ToList();
            }
            return listOrd;
        }
        public static Orders_user SelectOrdByID(int id)
        {
            Orders_user ord = new Orders_user();
            using (var context = new DataContext())
            {
                ord = context.Orders_user.Where(p => p.id_ord == id).FirstOrDefault();
            }
            return ord;
        }
        public static void UpdateStatus(int id, int status)
        {
            using (var context = new DataContext())
            {
                Orders_user orders = (from p in context.Orders_user
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
