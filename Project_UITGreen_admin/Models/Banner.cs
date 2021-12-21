using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project_UITGreen_admin.Models
{
    public class Banner
    {
        public int id_banner { set; get; }
        public string description { set; get; }
        public int id_img { set; get; }

        public static List<Banner> Select()
        {
            List<Banner> list = new List<Banner>();
            using (var context = new DataContext())
            {
                list = context.Banner.ToList();
            }
            return list;
        }
        public static Banner Selectone(int id)
        {
            Banner bn = new Banner();
            using (var context = new DataContext())
            {
                bn = context.Banner.Where(p=>p.id_banner==id).FirstOrDefault();
            }
            return bn;
        }
    }
}
