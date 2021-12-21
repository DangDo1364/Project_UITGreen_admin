using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project_UITGreen_admin.Models
{
    public class Image_product
    {
        public int id_img { get; set; }
        public int id_pro { get; set; }
        public string link { get; set; }
        public static List<Image_product> SelectImg()
        {
            List<Image_product> listImg = new List<Image_product>();
            using (var context = new DataContext())
            {
                listImg = context.Image_product.ToList();
            }
            return listImg;
        }
        public static List<Image_product> FindImgByIDPro(int id)
        {
            using (var context = new DataContext())
            {
                var listimg = context.Image_product;
                List<Image_product> list = (from p in listimg
                                            where (p.id_pro == id)
                                            select p).ToList();
                return list;
            }
        }
        public static Image_product FindImgByIDProOne(int id)
        {
            using (var context = new DataContext())
            {
                var listimg = context.Image_product;
                List<Image_product> list = (from p in listimg
                                            where (p.id_pro == id)
                                            select p).ToList();
                Image_product img = list.FirstOrDefault();
                return img;
            }
        }
        public static void InsertImg(int id_pro, string link)
        {
            using (var context = new DataContext())
            {
                context.Image_product.Add(new Image_product
                {
                    id_pro = id_pro,
                    link = link
                });
                context.SaveChanges();
            }
        }
        public static void UpdateImg(int id_img, string link)
        {
            using (var context = new DataContext())
            {
                var image = context.Image_product;
                Image_product img = (from p in image
                             where (p.id_img == id_img)
                             select p).FirstOrDefault();
                if (img != null)
                {
                    img.link = link;
                    context.SaveChanges();
                }
            }
        }
        public static void DeleteImg(int id)
        {
            using (var context = new DataContext())
            {
                var img = context.Image_product;
                var listimg = (from p in img
                             where (p.id_pro == id)
                             select p).ToList();
                if (listimg != null)
                {
                    foreach(var item in listimg)
                    {
                        context.Remove(item);
                    }    
                    context.SaveChanges();
                }
            }
        }

    }
}
