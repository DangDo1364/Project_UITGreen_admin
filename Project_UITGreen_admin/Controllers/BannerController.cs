using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Project_UITGreen_admin.Models;

namespace Project_UITGreen_admin.Controllers
{
    public class BannerController : Controller
    {
        public IActionResult Index()
        {
            var list = Banner.Select();
            return View(list);
        }
        public IActionResult Update(int id)
        {
            var bn = Banner.Selectone(id);
            return View(bn);
        }
        public IActionResult UpdateBanner(int id, string link)
        {
            Banner bn = Banner.Selectone(id);
            Image.UpdateImg(new Image {
            id_img = bn.id_img,
            link = link
            });
            return RedirectToAction("Index");
        }
    }
}
