using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Project_UITGreen_admin.Models;

namespace Project_UITGreen_admin.Models
{
    public class Comment
    {
        public int id_cmt { set; get; }
        public string name { set; get; }
        public int id_pro { set; get; }
        public int rate { set; get; }
        public string cmt_detail { set; get; }

        // Xử lý
        public static List<Comment> SelectCmt()
        {
            List<Comment> listCmt = new List<Comment>();
            using (var context = new DataContext())
            {
                listCmt = context.Comment.ToList();
            }
            return listCmt;
        }
        public static List<Comment> SelectOne(int id)
        {
            using (var context = new DataContext())
            {
                if (id != 0)
                {
                    List<Comment> cmt = (from p in context.Comment
                                         where (p.id_pro == id)
                                         select p).ToList();
                    return cmt;
                }
                else
                {
                    List<Comment> cmt = (from p in context.Comment
                                         select p).ToList();
                    return cmt;
                }
            }
        }
        public static void DeleteCmt(int id)
        {
            using (var context = new DataContext())
            {
                var cmt = context.Comment;
                var comment = (from p in cmt
                               where (p.id_cmt == id)
                               select p).FirstOrDefault();
                if (comment != null)
                {
                    context.Remove(comment);
                    context.SaveChanges();
                }
            }
        }
    }
}