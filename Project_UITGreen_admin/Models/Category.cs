using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Project_UITGreen_admin.Models;

namespace Project_UITGreen_admin.Models
{
    public class Category
    {
        public int id_cat { set; get; }

        public string name_cat { set; get; }

        public int id_parent { set; get; }

        public static void InsertCat(Category Cat)
        {
            using (var context = new DataContext())
            {
                context.Category.Add(new Category
                {
                    name_cat = Cat.name_cat,
                    id_parent = Cat.id_parent
                });
                context.SaveChanges();
            }
        }
        public static List<Category> SelectCat()
        {
            List<Category> listCat = new List<Category>();
            using (var context = new DataContext())
            {
                var a = context.Category;
                var cat = a.ToList();
                foreach (var cate in cat)
                {
                    listCat.Add(new Category()
                    {
                        id_cat = cate.id_cat,
                        name_cat = cate.name_cat,
                        id_parent = cate.id_parent
                    });
                }
            }
            return listCat;
        }
        public static List<Category> SearchCat(string search)
        {
            List<Category> listCat = new List<Category>();
            using (var context = new DataContext())
            {
                var a = context.Category;
                var cat = a
                .Where(p => p.name_cat.Contains(search)).ToList();
                foreach (var cate in cat)
                {
                    listCat.Add(new Category()
                    {
                        id_cat = cate.id_cat,
                        name_cat = cate.name_cat,
                        id_parent = cate.id_parent
                    });
                }
            }
            return listCat;
        }
        public static void DeleteCat(int id)
        {
            using (var context = new DataContext())
            {
                var cat = context.Category;
                var cate = (from p in cat
                            where (p.id_cat == id)
                            select p).FirstOrDefault();
                if (cate != null)
                {
                    context.Remove(cate);
                    context.SaveChanges();
                }
            }
        }
        public static void UpdateCat(Category category)
        {
            using (var context = new DataContext())
            {
                var cat = context.Category;
                Category cate = (from p in cat
                                 where (p.id_cat == category.id_cat)
                                 select p).FirstOrDefault();
                if (cate != null)
                {
                    cate.name_cat = category.name_cat;
                    cate.id_parent = category.id_parent;
                    context.SaveChanges();
                }

            }
        }
        public static Category FindCatByID(int id)
        {
            using (var context = new DataContext())
            {
                var cat = context.Category;
                Category cate = (from p in cat
                                 where (p.id_cat == id)
                                 select p).FirstOrDefault();
                return cate;
            }
        }
        public static List<Category> FindCatChildByIDParent(int id)
        {
            using (var context = new DataContext())
            {
                List<Category> cat = new List<Category>();
                if (id != 0)
                {
                    cat = context.Category.Where(p => p.id_parent == id).ToList();
                }
                else
                {
                    cat = context.Category.Where(p => p.id_parent > 0).ToList();
                }
                return cat;
            }
        }
        public static List<Category> FindCatChild()
        {
            List<Category> listCat = new List<Category>();
            using (var context = new DataContext())
            {
                var cat = context.Category;
                listCat = (from p in cat
                           where (p.id_parent > 0)
                           select p).ToList();
            }
            return listCat;
        }
        public static List<Category> FindParent()
        {
            List<Category> listCat = new List<Category>();
            using (var context = new DataContext())
            {
                var a = context.Category;
                var cat = a
                .Where(p => p.id_parent == 0);

                foreach (var cate in cat)
                {
                    listCat.Add(new Category()
                    {
                        id_cat = cate.id_cat,
                        name_cat = cate.name_cat,
                        id_parent = cate.id_parent
                    });
                }
            }
            return listCat;
        }

    }

}