using Aspose.Pdf;
using Aspose.Pdf.Text;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Project_UITGreen_admin.Models;

namespace Project_UITGreen_admin.Controllers
{
    public class OrdersUserController : Controller
    {
        public DataTable ConvertToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection properties =
               TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;
        }
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
        public IActionResult ExportSaleOrd(int id)
        {
            Orders_user ord1 = Orders_user.SelectOrdByID(id);
            Users u = Users.FindU(ord1.id_user);
            Order_status sta = Order_status.SelectOrdByID(ord1.id_ord);
            var document = new Document
            {
                PageInfo = new PageInfo { Margin = new MarginInfo(28, 28, 28, 40) }
            };
            var textFragment5 = new TextFragment("HÓA ĐƠN" + "\n\n");
            textFragment5.TextState.Font = FontRepository.FindFont("Arial");
            textFragment5.TextState.FontSize = 24;
            textFragment5.Position = new Position(240, 700);
            var textFragment = new TextFragment("Hóa đơn của khách hàng: " + u.fullname + "\n");
            var textFragment2 = new TextFragment("Địa chỉ: " + sta.address + "\n\n" + "Email: " + u.email + "\n\n" + "SĐT: " + u.phone + "\n");
            textFragment2.TextState.Font = FontRepository.FindFont("Arial");
            textFragment2.TextState.FontSize = 13;
            textFragment.TextState.Font = FontRepository.FindFont("Arial");
            textFragment.TextState.FontSize = 13;
            var textFragment3 = new TextFragment("\n");
            var textFragment4 = new TextFragment("Tiền ship: " + String.Format("{0:0,0 VNĐ}", ord1.ship) + "\n");
            textFragment4.TextState.Font = FontRepository.FindFont("Arial");
            textFragment4.TextState.FontSize = 13;
            var textFragment1 = new TextFragment("Tổng giá trị hóa đơn: " + String.Format("{0:0,0 VNĐ}", ord1.price_sum));
            textFragment1.TextState.Font = FontRepository.FindFont("Arial");
            textFragment1.TextState.FontSize = 13;
            Table table = new Table
            {
                ColumnWidths = "25% 25% 25% 25%",
                DefaultCellPadding = new MarginInfo(10, 5, 10, 5),
                Border = new BorderInfo(BorderSide.All, .5f, Color.Black),
                DefaultCellBorder = new BorderInfo(BorderSide.All, .2f, Color.Black),
                DefaultCellTextState =
                {
                    Font =  FontRepository.FindFont("Arial"),

                }

            };
            List<Order_user_items> ord = Order_user_items.SelectByID(id);
            List<Data> list = new List<Data>();
            foreach (var item in ord)
            {
                Product pro = Product.FindProByID(item.id_pro);
                list.Add(new Data
                {
                    NameProduct = pro.name_pro,
                    Quantity = item.quantity,
                    Type = pro.type,
                    Price = String.Format("{0:0,0 VNĐ}", item.price)
                });
            }

            DataTable dt = ConvertToDataTable(list);

            Page page = document.Pages.Add();
            var imageFileName = System.IO.Path.Combine("wwwroot/image/logouitgreen.png");
            page.AddImage(imageFileName, new Rectangle(20, 730, 120, 830));

            table.ImportDataTable(dt, true, 0, 0);

            document.Pages[1].AddImage(imageFileName, new Rectangle(20, 730, 120, 830));
            document.Pages[1].Paragraphs.Add(textFragment5);
            document.Pages[1].Paragraphs.Add(textFragment);
            document.Pages[1].Paragraphs.Add(textFragment2);
            document.Pages[1].Paragraphs.Add(textFragment4);
            document.Pages[1].Paragraphs.Add(table);
            document.Pages[1].Paragraphs.Add(textFragment3);
            document.Pages[1].Paragraphs.Add(textFragment1);


            using (var streamout = new MemoryStream())
            {
                document.Save(streamout);
                return new FileContentResult(streamout.ToArray(), "application/pdf")
                {
                    FileDownloadName = $"Hóa đơn {u.fullname}.pdf"
                };
            }
        }
    }
}
