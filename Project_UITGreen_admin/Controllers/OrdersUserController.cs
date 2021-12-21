using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Project_UITGreen_admin.Models;

namespace Project_UITGreen_admin.Controllers
{
    public class OrdersUserController : Controller
    {
        public IActionResult Received(int pg = 1)
        {
            const int pageSize = 5;
            if (pg < 1)
            {
                pg = 1;
            }
            int recsCount = Orders_user.SelectOrd(0).Count();
            var pager = new Pager(recsCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;
            var data = Orders_user.SelectOrd(0).Skip(recSkip).Take(pager.pageSize).ToList();
            this.ViewBag.Pager = pager;
            ViewBag.listord = data;

            return View();
        }
        public IActionResult ConfirmOrdUser(int id)
        {
            Orders_user.UpdateStatus(id, 1);
            Order_status.Insert(id, "Đã xác nhận", "");
            List<Order_user_items> list = Order_user_items.SelectByID(id);
            foreach (var item in list)
            {
                Storage_export.InsertExp(new Storage_export
                {
                    id_pro = item.id_pro,
                    quantity = item.quantity,
                    export_date = DateTime.Now,
                    reason = "Xuất kho giao hàng"
                });
            }
            return RedirectToAction("confirmed");
        }
        public IActionResult DeliveryUser(int id)
        {
            Orders_user.UpdateStatus(id, 2);
            Order_status.Insert(id, "Đang vận chuyển", "");
            return RedirectToAction("delivering");
        }
        public IActionResult accomplishUser(int id)
        {
            Orders_user.UpdateStatus(id, 3);
            Order_status.Insert(id, "Đã hoàn thành", "");
            return RedirectToAction("accomplished");
        }
        public IActionResult cancelOrdUser(int id)
        {
            Orders_user.UpdateStatus(id, 4);
            Order_status.Insert(id, "Đã hủy đơn", "");
            return RedirectToAction("cancelled");
        }
        public IActionResult Detail_ord(int id)
        {
            Orders_user ord = Orders_user.SelectOrdByID(id);
            List<Order_user_items> list = Order_user_items.SelectByID(id);
            this.ViewBag.ord = ord;
            this.ViewBag.listord = list;
            return View();
        }
        public IActionResult Confirmed(int pg = 1)
        {
            const int pageSize = 5;
            if (pg < 1)
            {
                pg = 1;
            }
            int recsCount = Orders_user.SelectOrd(1).Count();
            var pager = new Pager(recsCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;
            var data = Orders_user.SelectOrd(1).Skip(recSkip).Take(pager.pageSize).ToList();
            this.ViewBag.Pager = pager;
            ViewBag.listord = data;

            return View();
        }
        public IActionResult Delivering(int pg = 1)
        {
            const int pageSize = 5;
            if (pg < 1)
            {
                pg = 1;
            }
            int recsCount = Orders_user.SelectOrd(2).Count();
            var pager = new Pager(recsCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;
            var data = Orders_user.SelectOrd(2).Skip(recSkip).Take(pager.pageSize).ToList();
            this.ViewBag.Pager = pager;
            ViewBag.listord = data;

            return View();
        }
        public IActionResult Accomplished(int pg = 1)
        {
            const int pageSize = 5;
            if (pg < 1)
            {
                pg = 1;
            }
            int recsCount = Orders_user.SelectOrd(3).Count();
            var pager = new Pager(recsCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;
            var data = Orders_user.SelectOrd(3).Skip(recSkip).Take(pager.pageSize).ToList();
            this.ViewBag.Pager = pager;
            ViewBag.listord = data;

            return View();
        }
        public IActionResult Cancelled(int pg = 1)
        {
            const int pageSize = 5;
            if (pg < 1)
            {
                pg = 1;
            }
            int recsCount = Orders_user.SelectOrd(4).Count();
            var pager = new Pager(recsCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;
            var data = Orders_user.SelectOrd(4).Skip(recSkip).Take(pager.pageSize).ToList();
            this.ViewBag.Pager = pager;
            ViewBag.listord = data;

            return View();
        }
    }
}
