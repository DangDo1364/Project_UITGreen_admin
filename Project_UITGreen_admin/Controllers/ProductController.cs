using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Project_UITGreen_admin.Models;

namespace Project_UITGreen_admin.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index(int pg=1)
        {
            List<Product> listPro = new List<Product>();
            listPro = Product.SelectPro();
            List<Category> listCat = new List<Category>();
            foreach (var item1 in listPro)
            {
                listCat.Add(Category.FindCatByID(item1.id_cat));
            }
            this.ViewBag.listCat = listCat;

            const int pageSize = 5;
            if (pg < 1)
            {
                pg = 1;
            }
            int recsCount = listPro.Count();
            var pager = new Pager(recsCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;
            var data = listPro.OrderByDescending(p => p.id_pro).Skip(recSkip).Take(pager.pageSize).ToList();
            this.ViewBag.Pager = pager;
            return View(data);
        }
        [HttpGet]
        [Route("searchpro")]
        public IActionResult SearchPro(int pg = 1)
        {
            string search = Request.Query["search"].ToString();
            List<Product> listPro = new List<Product>();
            listPro = Product.SearchPro(search);
            List<Category> listCat = new List<Category>();
            foreach (var item1 in listPro)
            {
                listCat.Add(Category.FindCatByID(item1.id_cat));
            }
            this.ViewBag.listCat = listCat;

            const int pageSize = 5;
            if (pg < 1)
            {
                pg = 1;
            }
            int recsCount = listPro.Count();
            var pager = new Pager(recsCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;
            var data = listPro.OrderByDescending(p => p.id_pro).Skip(recSkip).Take(pager.pageSize).ToList();
            this.ViewBag.Pager = pager;
            this.ViewBag.search = search;
            return View("Index",data);
        }
        public IActionResult Insert()
        {
            List<Category> listCat = new List<Category>();
            listCat = Category.FindCatChild();
            return View(listCat);
        }
        public IActionResult Update(int id)
        {
            List<Category> listCat = new List<Category>();
            listCat = Category.FindCatChild();
            Product pro = Product.FindProByID(id);
            this.ViewBag.pro = pro;
            List <Image_product> listimg = new List<Image_product>();
            listimg = Image_product.FindImgByIDPro(id);
            this.ViewBag.listimg = listimg;
            return View(listCat);
        }
        public IActionResult UpdatePro(Product pro, int id1, int id2, int id3, int id4, int id5, string link1, string link2, string link3, string link4, string link5)
        {
            Product.UpdatePro(pro);
            Image_product.UpdateImg(id1, link1);
            Image_product.UpdateImg(id2, link2);
            Image_product.UpdateImg(id3, link3);
            Image_product.UpdateImg(id4, link4);
            Image_product.UpdateImg(id5, link5);
            return RedirectToAction("Index");
        }
        public IActionResult InsertPro(Product pro, string link1, string link2, string link3, string link4, string link5)
        {
            Product.InsertPro(pro);
            int id = Product.SelectProNew().id_pro;
            Image_product.InsertImg(id, link1);
            Image_product.InsertImg(id, link2);
            Image_product.InsertImg(id, link3);
            Image_product.InsertImg(id, link4);
            Image_product.InsertImg(id, link5);
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id)
        {
            Image_product.DeleteImg(id);
            Product.DeletePro(id);
            return RedirectToAction("Index");
        }
    }
}
