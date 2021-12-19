using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project_UITGreen_admin.Models
{
    public class Product
    {
        public int id_pro { set; get; }
        public string name_pro { set; get; }
        public int id_img { set; get; }
        public int id_cat { set; get; }
        public double price { set; get; }
        public int quantity { set; get; }
        public string origin { set; get; }
        public string status { set; get; }
        public string type { set; get; }
        public double discount { set; get; }
        public double sale_rate { set; get; }

        public static List<Product> SelectPro()
        {
            List<Product> listPro = new List<Product>();
            using (var context = new DataContext())
            {
                var product = context.Product.ToList();
                foreach (var pro in product)
                {
                    listPro.Add(new Product()
                    {
                        id_pro = pro.id_pro,
                        name_pro = pro.name_pro,
                        id_img = pro.id_img,
                        id_cat = pro.id_cat,
                        price = pro.price,
                        quantity = pro.quantity,
                        origin = pro.origin,
                        status = pro.status,
                        type = pro.type,
                        discount = pro.discount,
                        sale_rate = pro.sale_rate
                    });
                }
            }
            return listPro;
        }
        public static List<Product> SearchPro(string search)
        {
            List<Product> listPro = new List<Product>();
            using (var context = new DataContext())
            {
                var product = context.Product.Where(p => p.name_pro.Contains(search)).ToList();
                foreach (var pro in product)
                {
                    listPro.Add(new Product()
                    {
                        id_pro = pro.id_pro,
                        name_pro = pro.name_pro,
                        id_img = pro.id_img,
                        id_cat = pro.id_cat,
                        price = pro.price,
                        quantity = pro.quantity,
                        origin = pro.origin,
                        status = pro.status,
                        type = pro.type,
                        discount = pro.discount,
                        sale_rate = pro.sale_rate
                    });
                }
            }
            return listPro;
        }
        public static void InsertPro(Product pro, int id_img1)
        {
            using (var context = new DataContext())
            {
                context.Product.Add(new Product
                {
                    id_pro = pro.id_pro,
                    name_pro = pro.name_pro,
                    id_img = id_img1,
                    id_cat = pro.id_cat,
                    price = pro.price,
                    quantity = pro.quantity,
                    origin = pro.origin,
                    status = pro.status,
                    type = pro.type,
                    discount = pro.discount,
                    sale_rate = pro.sale_rate
                });
                context.SaveChanges();
            }
        }
        public static void UpdatePro(Product pro, string link1)
        {
            using (var context = new DataContext())
            {
                var product = context.Product;
                Product pro_duct = (from p in product
                                    where (p.id_pro == pro.id_pro)
                                    select p).FirstOrDefault();
                Image img = new Image();
                img.id_img = pro_duct.id_img;
                img.link = link1;
                Image.UpdateImg(img);
                if (pro_duct != null)
                {
                    pro_duct.name_pro = pro.name_pro;
                    pro_duct.id_cat = pro.id_cat;
                    pro_duct.price = pro.price;
                    pro_duct.quantity = pro.quantity;
                    pro_duct.origin = pro.origin;
                    pro_duct.status = pro.status;
                    pro_duct.type = pro.type;
                    pro_duct.discount = pro.discount;
                    pro_duct.sale_rate = pro.sale_rate;
                }
                context.SaveChanges();

            }
        }
        public static void UpdateImp(int id, int sl, double price)
        {
            using (var context = new DataContext())
            {
                var product = context.Product;
                Product pro_duct = (from p in product
                                    where (p.id_pro == id)
                                    select p).FirstOrDefault();
                if (pro_duct != null)
                {
                    pro_duct.price = price;
                    pro_duct.quantity = pro_duct.quantity + sl;
                }
                context.SaveChanges();
            }
        }
        public static void DeletePro(int id)
        {
            using (var context = new DataContext())
            {
                var pro = context.Product;
                var product = (from p in pro
                               where (p.id_pro == id)
                               select p).FirstOrDefault();
                if (product != null)
                {
                    context.Remove(product);
                    context.SaveChanges();
                }
            }
        }
        public static Product FindProByID(int id)
        {
            using (var context = new DataContext())
            {
                var pro = context.Product;
                Product product = (from p in pro
                                   where (p.id_pro == id)
                                   select p).FirstOrDefault();
                return product;
            }

        }
    }
}
