using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace Project_UITGreen_admin.Controllers
{
    public class CommentController : Controller
    {
        public IActionResult Index(int pg = 1)
        {
            const int pageSize = 5;
            if (pg < 1)
            {
                pg = 1;
            }
            int recsCount = Comment.SelectCmt().Count();
            var pager = new Pager(recsCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;
            var data = Comment.SelectCmt().Skip(recSkip).Take(pager.pageSize).ToList();
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
            int recsCount = Comment.SelectOne(search).Count();
            var pager = new Pager(recsCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;
            var data = Comment.SelectOne(search).Skip(recSkip).Take(pager.pageSize).ToList();
            this.ViewBag.Pager = pager;
            this.ViewBag.search = search;

            return View("index", data);
        }
        public IActionResult Delete(int id)
        {

            return View();
        }
    }
}