using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;
using Project_UITGreen_admin.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
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
                System.Drawing.Image img = System.Drawing.Image.FromFile(@"wwwroot\image\logohoadon.png");
                ExcelPicture pic = sheet.Drawings.AddPicture("logo", img);
                pic.SetPosition(0, 0);
                // Format cho đẹp
                sheet.DefaultColWidth = 17;
                sheet.DefaultRowHeight = 34;
                sheet.Cells.Style.WrapText = true;

                sheet.Row(1).Height = 62;
                using (var range = sheet.Cells["A1:C1"])
                {
                    range.Merge=true;
                }

                using (var range = sheet.Cells["D1:G1"])
                {
                    range.Value = $"BÁO CÁO ĐƠN HÀNG NGÀY {date.ToString("dd-MM-yyyy")}";
                    range.Merge = true;
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    range.Style.Font.SetFromFont(new Font("Times New Roman", 18));
                    range.Style.Font.Bold = true;
                    range.Style.Font.Color.SetColor(Color.Green);
                }

                using (var range = sheet.Cells["A2:G2"])
                {
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.Green);

                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    range.Style.Font.SetFromFont(new Font("Times New Roman", 12));
                    range.Style.Font.Bold = true;
                    range.Style.Font.Color.SetColor(Color.White);

                    range.Style.Border.Bottom.Style = ExcelBorderStyle.Thick;
                    range.Style.Border.Bottom.Color.SetColor(Color.DarkGreen);
                }

                using (var range = sheet.Cells["A3:G100"])
                {
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    range.Style.WrapText = true;
                    range.Style.Font.SetFromFont(new Font("Times New Roman", 12));
                }

                // add sheet 
                sheet.Cells[2, 1].Value = "Mã hóa đơn";
                sheet.Cells[2, 2].Value = "Tên khách hàng";
                sheet.Cells[2, 3].Value = "Tên mã giảm giá";
                sheet.Cells[2, 4].Value = "Tiền ship";
                sheet.Cells[2, 5].Value = "Kiểu thanh toán";
                sheet.Cells[2, 6].Value = "Ngày đặt";
                sheet.Cells[2, 7].Value = "Tổng hóa đơn";

                int row = 3;
                double price_sum = 0;
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
                    sheet.Cells[row, 4].Value = String.Format("{0:0,0 VNĐ}", item.ship);
                    sheet.Cells[row, 5].Value = type;
                    sheet.Cells[row, 6].Value = item.date.ToString("dd/MM/yyyy HH:mm:ss");
                    sheet.Cells[row, 7].Value = String.Format("{0:0,0 VNĐ}", item.price_sum);
                    row++;
                    price_sum += item.price_sum;
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
                    sheet.Cells[row, 4].Value = String.Format("{0:0,0 VNĐ}", item.ship);
                    sheet.Cells[row, 5].Value = type;
                    sheet.Cells[row, 6].Value = item.date.ToString("dd/MM/yyyy HH:mm:ss");
                    sheet.Cells[row, 7].Value = String.Format("{0:0,0 VNĐ}", item.price_sum);
                    row++;
                    price_sum += item.price_sum;
                }
                sheet.Cells[row,6].Style.Font.Bold = true;
                sheet.Cells[row,7].Style.Font.Bold = true;
                sheet.Cells[row, 6].Value = "Tổng tiền";
                sheet.Cells[row, 7].Value = String.Format("{0:0,0 VNĐ}", price_sum);
                package.Save();
            }
            stream.Position = 0;
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                , fileName);
        }


    }
}
