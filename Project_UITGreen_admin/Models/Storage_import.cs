using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project_UITGreen_admin.Models
{
    public class Storage_import
    {
        public int id_imp { set; get; }
        public int id_pro { set; get; }
        public DateTime import_date { set; get; }
        public int quantity { set; get; }
        public double first_price { set; get; }
        public string reason { set; get; }
        public static List<Storage_import> SelectImp()
        {
            List<Storage_import> listImp = new List<Storage_import>();
            using (var context = new DataContext())
            {
                listImp = context.Storage_import.OrderByDescending(s => s.id_imp).ToList();
            }
            return listImp;
        }
        public static void InsertImp(Storage_import n)
        {
            using (var context = new DataContext())
            {
                context.Storage_import.Add(new Storage_import
                {
                    id_imp = n.id_imp,
                    id_pro = n.id_pro,
                    import_date = DateTime.Now,
                    quantity = n.quantity,
                    first_price = n.first_price,
                    reason = n.reason
                });
                context.SaveChanges();
            }
        }
    }
}
