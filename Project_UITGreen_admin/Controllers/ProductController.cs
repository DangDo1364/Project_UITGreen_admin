using Microsoft.AspNetCore.Mvc;
using Project_UITGreen_admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project_UITGreen_admin.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index(int pg = 1)
        {
            List<Product> listPro = new List<Product>();
            listPro = Product.SelectPro();
            List<Image> listImg = new List<Image>();
            foreach (var item in listPro)
            {
                listImg.Add(Image.SelectOne(item.id_img));
            }
            this.ViewBag.listImg = listImg;
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
            var data = listPro.Skip(recSkip).Take(pager.pageSize).ToList();
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
            List<Image> listImg = new List<Image>();
            foreach (var item in listPro)
            {
                listImg.Add(Image.SelectOne(item.id_img));
            }
            this.ViewBag.listImg = listImg;
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
            var data = listPro.Skip(recSkip).Take(pager.pageSize).ToList();
            this.ViewBag.Pager = pager;
            this.ViewBag.search = search;
            return View("Index", data);
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
            Image img = Image.SelectOne(pro.id_img);
            this.ViewBag.img = img;
            return View(listCat);
        }
        public IActionResult UpdatePro(Product pro, string link)
        {
            Product.UpdatePro(pro, link);
            return RedirectToAction("Index");
        }
        public IActionResult InsertPro(Image img, Product pro)
        {
            Image.InsertImg(img);
            Image image = Image.SelectNew();
            Product.InsertPro(pro, image.id_img);
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id)
        {
            Product pro = Product.FindProByID(id);
            int id_img = pro.id_img;
            Product.DeletePro(id);
            Image.DeleteImg(id_img);
            return RedirectToAction("Index");
        }
    }
}
