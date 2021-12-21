using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Project_UITGreen_admin.Models;
using Aspose.Pdf;
using Aspose.Pdf.Text;
using System.Data;
using System.IO;
using System.ComponentModel;

namespace Project_UITGreen_admin.Controllers
{
    public class OrdersController : Controller
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
            int recsCount = Orders.SelectOrd(0).Count();
            var pager = new Pager(recsCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;
            var data = Orders.SelectOrd(0).Skip(recSkip).Take(pager.pageSize).ToList();
            this.ViewBag.Pager = pager;
            ViewBag.listord = data;

            return View();
        }
        public IActionResult ConfirmOrd(int id)
        {
            Orders.UpdateStatus(id,1);
            List<Order_items> list = Order_items.SelectByID(id);
            foreach(var item in list)
            {
                Storage_export.InsertExp(new Storage_export
                {
                    id_pro = item.id_pro,
                    quantity = item.quantity,
                    export_date = DateTime.Now,
                    reason = "Xuất kho giao hàng"
                }) ;
            }           
            return RedirectToAction("confirmed");
        }
        public IActionResult Delivery(int id)
        {
            Orders.UpdateStatus(id, 2);
            return RedirectToAction("delivering");
        }
        public IActionResult accomplish(int id)
        {
            Orders.UpdateStatus(id, 3);
            return RedirectToAction("accomplished");
        }
        public IActionResult cancelOrd(int id)
        {
            Orders.UpdateStatus(id, 4);
            return RedirectToAction("cancelled");
        }
        public IActionResult Detail_ord(int id)
        {
            Orders ord = Orders.SelectOrdByID(id);
            List<Order_items> list = Order_items.SelectByID(id);
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
            int recsCount = Orders.SelectOrd(1).Count();
            var pager = new Pager(recsCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;
            var data = Orders.SelectOrd(1).Skip(recSkip).Take(pager.pageSize).ToList();
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
            int recsCount = Orders.SelectOrd(2).Count();
            var pager = new Pager(recsCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;
            var data = Orders.SelectOrd(2).Skip(recSkip).Take(pager.pageSize).ToList();
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
            int recsCount = Orders.SelectOrd(3).Count();
            var pager = new Pager(recsCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;
            var data = Orders.SelectOrd(3).Skip(recSkip).Take(pager.pageSize).ToList();
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
            int recsCount = Orders.SelectOrd(4).Count();
            var pager = new Pager(recsCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;
            var data = Orders.SelectOrd(4).Skip(recSkip).Take(pager.pageSize).ToList();
            this.ViewBag.Pager = pager;
            ViewBag.listord = data;

            return View();
        }
        public IActionResult ExportSaleOrd(int id=10)
        {
            Orders ord1 = Orders.SelectOrdByID(id);
            Customer cus = Customer.SelectByID(ord1.id_customer);
            var document = new Document
            {
                PageInfo = new PageInfo {Margin = new MarginInfo(28,28,28,40)}
            };
            var pdfpage = document.Pages.Add();

            var textFragment = new TextFragment("Hóa đơn của khách hàng "+ cus.name_cus );
            var textFragment1 = new TextFragment("Tổng giá trị hóa đơn " + String.Format("{0:0,0 VNĐ}", ord1.price_sum));
            Table table = new Table
            {
                ColumnWidths = "25% 25% 25% 25%",
                DefaultCellPadding = new MarginInfo(10, 5, 10, 5),
                Border = new BorderInfo(BorderSide.All, .5f, Color.Black),
                DefaultCellBorder = new BorderInfo(BorderSide.All,.2f,Color.Black)
            };
            List<Order_items> ord = Order_items.SelectByID(id);
            
            DataTable dt = ConvertToDataTable(ord);

            table.ImportDataTable(dt,true,0,0);
            document.Pages[1].Paragraphs.Add(textFragment);
            document.Pages[1].Paragraphs.Add(table);
            document.Pages[1].Paragraphs.Add(textFragment1);

            using (var streamout = new MemoryStream())
            {
                document.Save(streamout);
                return new FileContentResult(streamout.ToArray(), "application/pdf")
                {
                    FileDownloadName=$"HoaDon{cus.name_cus}.pdf"
                }; 
            }    
        }
    }
}
