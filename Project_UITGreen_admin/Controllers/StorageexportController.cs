using Microsoft.AspNetCore.Mvc;
using Project_UITGreen_admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project_UITGreen_admin.Controllers
{
    public class StorageexportController : Controller
    {
        public IActionResult Index(int pg = 1)
        {
            const int pageSize = 6;
            if (pg < 1)
            {
                pg = 1;
            }
            int recsCount = Storage_export.SelectExp().Count();
            var pager = new Pager(recsCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;
            var data = Storage_export.SelectExp().Skip(recSkip).Take(pager.pageSize).ToList();
            this.ViewBag.Pager = pager;

            return View(data);
        }
        public IActionResult Insert()
        {
            List<Product> listPro = Product.SelectPro();
            return View(listPro);
        }
        public IActionResult InsertExp(Storage_export n)
        {
            Storage_export.InsertExp(n);
            Product.UpdateExp(n.id_pro, n.quantity);
            return RedirectToAction("Index");
        }
    }
}
