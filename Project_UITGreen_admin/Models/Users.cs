using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project_UITGreen_admin.Models
{
    public class Users
    {
        public int id { set; get; }
        public string fullname { set; get; }
        public string password { set; get; }
        public string email { set; get; }
        public string address { set; get; }
        public string phone { set; get; }
        public static List<Users> SelectU()
        {
            List<Users> list = new List<Users>();
            using (var context = new DataContext())
            {
                var list1 = context.Users.ToList();
                foreach (var item in list1)
                {
                    list.Add(new Users()
                    {
                        id = item.id,
                        fullname =  item.fullname,
                        password = item.password,
                        email = item.email,
                        address = item.address,
                        phone = item.phone
                    });
                }
            }
            return list;
        }
        public static Users FindU(int id)
        {
            Users u = new Users();
            using (var context = new DataContext())
            {
                u = (from p in context.Users
                     where (p.id == id)
                     select p).FirstOrDefault();
            }
            return u;
        }
        public static void InsertU(Users item)
        {
            using (var context = new DataContext())
            {
                context.Users.Add(new Users
                {
                    id = item.id,
                    fullname = item.fullname,
                    password = item.password,
                    email = item.email,
                    address = item.address,
                    phone = item.phone
                });
                context.SaveChanges();
            }
        }
        public static void UpdateU(Users item)
        {
            using (var context = new DataContext())
            {
                Users user = (from p in context.Users
                               where (p.id == item.id)
                               select p).FirstOrDefault();
                if (user != null)
                {
                    user.id = item.id;
                    user.fullname = item.fullname;
                    user.password = item.password;
                    user.email = item.email;
                    user.address = item.address;
                    user.phone = item.phone;
                    context.SaveChanges();
                }
            }
        }
    }
}
