using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OfficeOpenXml;
using Project_UITGreen_admin.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Project_UITGreen_admin.Controllers
{
    public class StatisticalController : Controller
    {
        public class Product_Ex
        {
            public int id { set; get; }
            public int quantity { set; get; }
        }
        public class User_Ex
        {
            public int id { set; get; }
            public double price { set; get; }
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Revenue(string year, string type = "Normal")
        {
            double total = 0;
            double[] totalprice = new double[12];
            string[] datetime = { "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12" };
            for (int i = 0; i < datetime.Length; i++)
            {
                string date = datetime[i] + "-" + year;
                List<Orders> list = Orders.Select().Where(p => p.date.ToString("dd-MM-yyyy").Contains(date) && p.status > 0 && p.status < 4).ToList();
                List<Orders> list1 = Orders.Select().Where(p => p.date.ToString("dd-MM-yyyy").Contains(date) && p.status > 0 && p.status < 4).ToList();
                total = list.Sum(p => p.price_sum) + list1.Sum(p => p.price_sum);
                totalprice[i] = total;
            }

            ViewBag.year = year;

            if (type == "ajax")
            {
                return Json(new
                {

                    date = datetime,
                    total = totalprice
                });
            }
            return View();
        }
        public IActionResult Product_Quantity(string type = "Normal")
        {
            var sells = Storage_export.Select().Where(p => p.reason.Equals("Xuất kho giao hàng")).GroupBy(a => a.id_pro)
            .Select(a => new Product_Ex { id = a.Key, quantity = a.Sum(b => b.quantity) })
            .OrderByDescending(a => a.quantity).Take(20)
            .ToList();

            List<string> name_pro = new List<string>();
            List<double> quan = new List<double>();

            for (int i = 0; i < sells.Count; i++)
            {
                Product pro = Product.FindProByID(sells[i].id);
                name_pro.Add(pro.name_pro);
                quan.Add(sells[i].quantity);
            }
            if (type == "ajax")
            {
                return Json(new
                {
                    name_pro = name_pro,
                    quantity = quan
                });
            }
            return View();
        }
        public IActionResult Top_customer(string type = "Normal")
        {
            var sells = Orders_user.SelectOrdTop().GroupBy(a => a.id_user)
            .Select(a => new User_Ex { id = a.Key, price = a.Sum(b => b.price_sum) })
            .OrderByDescending(a => a.price).Take(5).ToList();

            List<string> name_user = new List<string>();
            List<double> price = new List<double>();

            for (int i = 0; i < sells.Count; i++)
            {
                Users u = Users.FindU(sells[i].id);
                name_user.Add(u.fullname);
                price.Add(sells[i].price);
            }
            if (type == "ajax")
            {
                return Json(new
                {
                    name_user = name_user,
                    price = price
                });
            }
            return View();
        }

        public IActionResult Export(DateTime date)
        {
            string date1 = date.ToString("dd-MM-yyyy");
            ExcelPackage.LicenseContext = LicenseContext.Commercial;
            var list = Orders.SelectDate(date);
            var list1 = Orders_user.SelectDate(date);
            var stream = new MemoryStream();
            var fileName = $"HoaDon_{date.ToString("dd-MM-yyyy")}.xlsx";
            using (var package = new ExcelPackage(stream))
            {
                var sheet = package.Workbook.Worksheets.Add("HoaDon");
                // add sheet 
                sheet.Cells[1, 1].Value = "Mã hóa đơn";
                sheet.Cells[1, 2].Value = "Tên khách hàng";
                sheet.Cells[1, 3].Value = "Tên mã giảm giá";
                sheet.Cells[1, 4].Value = "Tiền ship";
                sheet.Cells[1, 5].Value = "Kiểu thanh toán";
                sheet.Cells[1, 6].Value = "Ngày đặt";
                sheet.Cells[1, 7].Value = "Tổng hóa đơn";

                int row = 2;
                foreach (var item in list)
                {
                    string type = "";
                    if (item.paymethod == 1)
                    {
                        type = "Tiền mặt";
                    }
                    else
                    {
                        type = "Thanh toán online";
                    }
                    Customer cus = Customer.SelectByID(item.id_customer);
                    Promotion pro = Promotion.selectbyid(item.id_promotion);
                    sheet.Cells[row, 1].Value = item.id_ord;
                    sheet.Cells[row, 2].Value = cus.name_cus;
                    sheet.Cells[row, 3].Value = pro.name_promotion;
                    sheet.Cells[row, 4].Value = item.ship;
                    sheet.Cells[row, 5].Value = type;
                    sheet.Cells[row, 6].Value = item.date.ToString();
                    sheet.Cells[row, 7].Value = String.Format("{0:0,0 VNĐ}", item.price_sum);
                    row++;
                }
                foreach (var item in list1)
                {
                    string type = "";
                    if (item.paymethod == 1)
                    {
                        type = "Tiền mặt";
                    }
                    else
                    {
                        type = "Thanh toán online";
                    }
                    Users u = Users.FindU(item.id_user);
                    Promotion pro = Promotion.selectbyid(item.id_promotion);
                    sheet.Cells[row, 1].Value = item.id_ord;
                    sheet.Cells[row, 2].Value = u.fullname;
                    sheet.Cells[row, 3].Value = pro.name_promotion;
                    sheet.Cells[row, 4].Value = item.ship;
                    sheet.Cells[row, 5].Value = type;
                    sheet.Cells[row, 6].Value = item.date.ToString();
                    sheet.Cells[row, 7].Value = String.Format("{0:0,0 VNĐ}", item.price_sum);
                    row++;
                }
                package.Save();
            }
            stream.Position = 0;
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                , fileName);
        }


    }
}
