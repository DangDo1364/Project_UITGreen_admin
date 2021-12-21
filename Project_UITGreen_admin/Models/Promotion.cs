using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project_UITGreen_admin.Models
{
    public class Promotion
    {
        public int id_promotion { set; get; }
        public string name_promotion { set; get; }
        public double discount { set; get; }
        public DateTime date { set; get; }
        public static List<Promotion> select()
        {
            using (var context = new DataContext())
            {
                var pro = context.Promotion.Where(p => p.id_promotion > 1).ToList();
                return pro;
            }
        }
        public static Promotion selectbyid(int id)
        {
            using (var context = new DataContext())
            {
                var pro = context.Promotion;
                Promotion pro1 = (from p in pro
                                  where (p.id_promotion == id)
                                  select p).FirstOrDefault();
                return pro1;
            }
        }
        public static void Insert(Promotion pro)
        {
            using (var context = new DataContext())
            {
                context.Promotion.Add(new Promotion
                {
                    name_promotion = pro.name_promotion,
                    discount = pro.discount,
                    date = pro.date
                });
                context.SaveChanges();
            }
        }
        public static void Update(Promotion promo)
        {
            using (var context = new DataContext())
            {
                var pro = (from p in context.Promotion
                           where (p.id_promotion == promo.id_promotion)
                           select p).FirstOrDefault();
                if (pro != null)
                {
                    pro.name_promotion = promo.name_promotion;
                    pro.discount = promo.discount;
                    pro.date = promo.date;
                    context.SaveChanges();
                }
            }
        }

    }
}

