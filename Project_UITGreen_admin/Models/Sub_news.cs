using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project_UITGreen_admin.Models
{
    public class Sub_news
    {
        public int id_sub { set; get; }
        public string email { set; get; }

        public static List<Sub_news> Select()
        {
            List<Sub_news> list = new List<Sub_news>();
            using (var context = new DataContext())
            {
                list = context.Sub_news.ToList();
            }
            return list;
        }
    }
}
