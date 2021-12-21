using Microsoft.AspNetCore.Mvc;
using Project_UITGreen_admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project_UITGreen_admin.Controllers
{
    public class CategoryController : Controller
    {
        public IActionResult Index(int pg = 1)
        {
            const int pageSize = 5;
            if (pg < 1)
            {
                pg = 1;
            }
            int recsCount = Category.FindCatChild().Count();
            var pager = new Pager(recsCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;
            var data = Category.FindCatChild().Skip(recSkip).Take(pager.pageSize).ToList();
            this.ViewBag.Pager = pager;

            return View(data);
        }

        public IActionResult Search(int search, int pg = 1)
        {
            const int pageSize = 5;
            if (pg < 1)
            {
                pg = 1;
            }
            int recsCount = Category.FindCatChildByIDParent(search).Count();
            var pager = new Pager(recsCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;
            var data = Category.FindCatChildByIDParent(search).Skip(recSkip).Take(pager.pageSize).ToList();
            this.ViewBag.Pager = pager;
            this.ViewBag.search = search;

            return View("Index", data);
        }
        public IActionResult InsertCat(Category cat)
        {
            Category.InsertCat(cat);
            return RedirectToAction("Index");
        }
        public IActionResult Insert()
        {
            List<Category> listCat = Category.FindParent();
            return View(listCat);
        }
        public IActionResult Update(int id)
        {
            List<Category> listCat = Category.FindParent();
            Category cat = Category.FindCatByID(id);
            this.ViewBag.Cat = cat;
            return View(listCat);
        }
        public IActionResult UpdateCat(Category cat)
        {
            Category.UpdateCat(cat);
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id)
        {
            Category.DeleteCat(id);
            return RedirectToAction("Index");
        }
    }
}