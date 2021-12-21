using System.Collections.Generic;
using System.Linq;

namespace Project_UITGreen_admin.Models
{
    public class Product
    {
        public int id_pro { set; get; }
        public string name_pro { set; get; }
        public int id_cat { set; get; }
        public double price { set; get; }
        public int quantity { set; get; }
        public string origin { set; get; }
        public string status { set; get; }
        public string type { set; get; }
        public double discount { set; get; }
        public double sale_rate { set; get; }

        // xử lý
        public static List<Product> SelectPro()
        {
            List<Product> listPro = new List<Product>();
            using (var context = new DataContext())
            {
                listPro = context.Product.ToList();
            }
            return listPro;
        }
        public static Product SelectProNew()
        {
            Product Pro = new Product();
            using (var context = new DataContext())
            {
                Pro = context.Product.ToList().Last();
            }
            return Pro;
        }
        public static List<Product> SearchPro(string search)
        {
            List<Product> listPro = new List<Product>();
            using (var context = new DataContext())
            {
                listPro = context.Product.Where(p => p.name_pro.Contains(search)).ToList();
            }
            return listPro;
        }
        public static void InsertPro(Product pro)
        {
            using (var context = new DataContext())
            {
                context.Product.Add(new Product
                {
                    id_pro = pro.id_pro,
                    name_pro = pro.name_pro,
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
        public static void UpdatePro(Product pro)
        {
            using (var context = new DataContext())
            {
                var product = context.Product;
                Product pro_duct = (from p in product
                                    where (p.id_pro == pro.id_pro)
                                    select p).FirstOrDefault();
            
                if (pro_duct != null)
                {
                    pro_duct.name_pro = pro.name_pro;
                    pro_duct.id_cat = pro.id_cat;
                    pro_duct.price = pro.price;
                    pro_duct.origin = pro.origin;
                    pro_duct.status = pro.status;
                    pro_duct.type = pro.type;
                    pro_duct.discount = pro.discount;
                    pro_duct.sale_rate = pro.sale_rate;
                }
                context.SaveChanges();

            }
        }
        public static void UpdateImp(int id, int sl ,double price)
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
        public static void UpdateExp(int id, int sl)
        {
            using (var context = new DataContext())
            {
                var product = context.Product;
                Product pro_duct = (from p in product
                                    where (p.id_pro == id)
                                    select p).FirstOrDefault();
                if (pro_duct != null)
                {
                    pro_duct.quantity = pro_duct.quantity - sl;
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
