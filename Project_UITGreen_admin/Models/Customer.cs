using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project_UITGreen_admin.Models
{
    public class Customer
    {
        public int id_cus { set; get; }
        public string name_cus { set; get; }
        public string address { set; get; }
        public string email { set; get; }
        public string phone { set; get; }

        public static List<Customer> SelectCus()
        {
            List<Customer> listCus = new List<Customer>();
            using (var context = new DataContext())
            {
                listCus = context.Customer.ToList();
            }
            return listCus;
        }
        public static Customer SelectByID(int id)
        {
            Customer cus = new Customer();
            using (var context = new DataContext())
            {
                cus = context.Customer.Where(p => p.id_cus == id).FirstOrDefault();
            }
            return cus;
        }
        public static void InsertCus(Customer c)
        {
            using (var context = new DataContext())
            {
                context.Customer.Add(new Customer
                {
                    id_cus = c.id_cus,
                    name_cus = c.name_cus,
                    address = c.address,
                    phone = c.phone
                });
                context.SaveChanges();
            }
        }
    }
}
