using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project_UITGreen_admin.Models
{
    public class Storage_export
    {
        public int id_exp { set; get; }
        public int id_pro { set; get; }
        public DateTime export_date { set; get; }
        public int quantity { set; get; }
        public string reason { set; get; }
        public static List<Storage_export> SelectExp()
        {
            List<Storage_export> listexp = new List<Storage_export>();
            using (var context = new DataContext())
            {
                listexp = context.Storage_export.OrderByDescending(s => s.id_exp).ToList();
            }
            return listexp;
        }
        public static List<Storage_export> Select()
        {
            List<Storage_export> listexp = new List<Storage_export>();
            using (var context = new DataContext())
            {
                listexp = context.Storage_export.ToList();
            }
            return listexp;
        }
        public static void InsertExp(Storage_export n)
        {
            using (var context = new DataContext())
            {
                context.Storage_export.Add(new Storage_export
                {
                    id_exp = n.id_exp,
                    id_pro = n.id_pro,
                    export_date = DateTime.Now,
                    quantity = n.quantity,
                    reason = n.reason
                });
                context.SaveChanges();
            }
        }
    }
}
