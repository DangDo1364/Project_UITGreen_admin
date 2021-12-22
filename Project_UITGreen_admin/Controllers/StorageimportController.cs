using Microsoft.AspNetCore.Mvc;
using Project_UITGreen_admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project_UITGreen_admin.Controllers
{
    public class StorageimportController : Controller
    {
        public IActionResult Index(int pg = 1)
        {
            const int pageSize = 6;
            if (pg < 1)
            {
                pg = 1;
            }
            int recsCount = Storage_import.SelectImp().Count();
            var pager = new Pager(recsCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;
            var data = Storage_import.SelectImp().Skip(recSkip).Take(pager.pageSize).ToList();
            this.ViewBag.Pager = pager;

            return View(data);
        }
        public IActionResult Insert()
        {
            List<Product> listPro = Product.SelectPro();
            return View(listPro);
        }
        public IActionResult InsertImp(Storage_import n)
        {
            Storage_import.InsertImp(n);
            Product.UpdateImp(n.id_pro, n.quantity, n.first_price);
            return RedirectToAction("Index");
        }
    }
}
