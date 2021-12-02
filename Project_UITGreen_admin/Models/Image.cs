using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project_UITGreen_admin.Models
{
    public class Image
    {
        public int id_img { set; get; }
        public string link { set; get; }

        // xử lý
        public static List<Image> SelectImg()
        {
            List<Image> listImg = new List<Image>();
            using (var context = new DataContext())
            {
                var img = context.Image.ToList();
                foreach (var image in img)
                {
                    listImg.Add(new Image()
                    {
                        id_img = image.id_img,
                        link = image.link
                    });
                }
            }
            return listImg;
        }
        public static void InsertImg(Image img)
        {
            using (var context = new DataContext())
            {
                context.Image.Add(new Image
                {
                    id_img = img.id_img,
                    link = img.link
                });
                context.SaveChanges();
            }
        }
        public static void UpdateImg(Image img1)
        {
            using (var context = new DataContext())
            {
                var image = context.Image;
                Image img = (from p in image
                             where (p.id_img == img1.id_img)
                             select p).FirstOrDefault();
                if (img != null)
                {
                    img.link = img1.link;
                    context.SaveChanges();
                }
            }
        }
        public static Image SelectOne(int id)
        {
            using (var context = new DataContext())
            {
                var image = context.Image;
                Image img = (from p in image
                             where (p.id_img == id)
                             select p).FirstOrDefault();
                return img;
            }
        }
        public static Image SelectNew()
        {
            using (var context = new DataContext())
            {
                Image image = context.Image.ToList().Last();
                return image;
            }
        }
        public static void DeleteImg(int id)
        {
            using (var context = new DataContext())
            {
                var img = context.Image;
                var image = (from p in img
                             where (p.id_img == id)
                             select p).FirstOrDefault();
                if (image != null)
                {
                    context.Remove(image);
                    context.SaveChanges();
                }
            }
        }
    }
}
