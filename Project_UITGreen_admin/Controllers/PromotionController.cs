using Microsoft.AspNetCore.Mvc;
using Project_UITGreen_admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project_UITGreen_admin.Controllers
{
    public class PromotionController : Controller
    {
        public IActionResult Index()
        {
            var list = Promotion.select();
            ViewBag.list = list;
            return View();
        }
        public IActionResult Insert()
        {
            return View();
        }
        public IActionResult Update(int id)
        {
            Promotion pro = Promotion.selectbyid(id);
            ViewBag.pro = pro;
            return View();
        }
        public IActionResult InsertPro(Promotion pro)
        {
            Promotion.Insert(pro);
            return RedirectToAction("Index");
        }
        public IActionResult UpdatePro(Promotion pro)
        {
            Promotion.Update(pro);
            return RedirectToAction("Index");
        }
    }
}
