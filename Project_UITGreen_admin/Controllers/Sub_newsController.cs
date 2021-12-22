using Microsoft.AspNetCore.Mvc;
using Project_UITGreen_admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project_UITGreen_admin.Controllers
{
    public class Sub_newsController : Controller
    {
        public IActionResult Index()
        {
            List<Sub_news> list = Sub_news.Select();
            ViewBag.list = list;
            return View();
        }
        public IActionResult SendMail()
        {
            return View();
        }
    }
}
