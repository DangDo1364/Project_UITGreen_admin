using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Project_UITGreen_admin.Models;
//using Aspose.Pdf;
using Aspose.Pdf.Text;
using System.Data;
using System.IO;
using System.ComponentModel;
using Syncfusion.XlsIO;
using Syncfusion.Drawing;

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
            Orders.UpdateStatus(id, 1);
            List<Order_items> list = Order_items.SelectByID(id);
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

        /*public IActionResult ExportSaleOrd(int id)
        {
            Orders ord1 = Orders.SelectOrdByID(id);
            Customer cus = Customer.SelectByID(ord1.id_customer);
            var document = new Document
            {
                PageInfo = new PageInfo { Margin = new MarginInfo(28, 28, 28, 40) }
            };
            var textFragment5 = new TextFragment("HÓA ĐƠN" + "\n\n");
            textFragment5.TextState.Font = FontRepository.FindFont("Arial");
            textFragment5.TextState.FontSize = 24;
            textFragment5.Position = new Position(240, 700);
            var textFragment = new TextFragment("Hóa đơn của khách hàng: " + cus.name_cus + "\n");
            var textFragment2 = new TextFragment("Địa chỉ: " + cus.address + "\n\n" + "Email: " + cus.email + "\n\n" + "SĐT: " + cus.phone + "\n");
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
            List<Order_items> ord = Order_items.SelectByID(id);
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
                    FileDownloadName = $"Hóa đơn {cus.name_cus}.pdf"
                };
            }
        }
        */
        public IActionResult CreateDocument()
        {
            using (ExcelEngine excelEngine = new ExcelEngine())
            {
                IApplication application = excelEngine.Excel;

                application.DefaultVersion = ExcelVersion.Xlsx;

                //Tạo workbook
                IWorkbook workbook = application.Workbooks.Create(1);
                IWorksheet worksheet = workbook.Worksheets[0];

                //Logo
                FileStream imageStream = new FileStream("wwwroot/image/logohoadon.png", FileMode.Open, FileAccess.Read);
                IPictureShape shape = worksheet.Pictures.AddPicture(1, 1, imageStream);

                //Xóa gridline
                worksheet.IsGridLinesVisible = false;

                //Thông tin giới thiệu
                worksheet.Range["A3"].Text = "Trường Đại học Công nghệ Thông tin - ĐHQG TP.HCM";
                worksheet.Range["A4"].Text = "Khu phố 6, phường Linh Trung, TP.Thủ Đức, TP.HCM";
                worksheet.Range["A5"].Text = "Phone: 0123456789";

                //In đậm text
                worksheet.Range["A3:A5"].CellStyle.Font.Bold = true;

                //Merge cells
                worksheet.Range["D1:E1"].Merge();

                //Enter text to the cell D1 and apply formatting.
                worksheet.Range["D1"].Text = "HÓA ĐƠN";
                worksheet.Range["D1"].CellStyle.Font.Bold = true;
                worksheet.Range["D1"].CellStyle.Font.RGBColor = Color.FromArgb(0, 137, 71);
                worksheet.Range["D1"].CellStyle.Font.Size = 35;
                worksheet.Range["D1"].CellStyle.Font.FontName = "Arial";

                //Apply alignment in the cell D1
                worksheet.Range["D1"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignRight;
                worksheet.Range["D1"].CellStyle.VerticalAlignment = ExcelVAlign.VAlignTop;

                //Thông tin đơn hàng
                worksheet.Range["D5"].Text = "MÃ ĐƠN HÀNG";
                worksheet.Range["E5"].Text = "NGÀY LẬP";
                worksheet.Range["D6"].Number = 1; 
                worksheet.Range["E6"].Value = "1/1/2022";
                worksheet.Range["D7"].Text = "MÃ KHÁCH HÀNG";
                worksheet.Range["E7"].Text = "THANH TOÁN";
                worksheet.Range["D8"].Number = 2;
                worksheet.Range["E8"].Text = "Tiền mặt";

                //Format
                worksheet.Range["D5:E5"].CellStyle.Color = Color.FromArgb(0, 137, 71);
                worksheet.Range["D7:E7"].CellStyle.Color = Color.FromArgb(0, 137, 71);

                worksheet.Range["D5:E5"].CellStyle.Font.Color = ExcelKnownColors.White;
                worksheet.Range["D7:E7"].CellStyle.Font.Color = ExcelKnownColors.White;

                worksheet.Range["D5:E8"].CellStyle.Font.Bold = true;

                worksheet.Range["D5:E8"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                worksheet.Range["D5:E5"].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                worksheet.Range["D7:E7"].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                worksheet.Range["D6:E6"].CellStyle.VerticalAlignment = ExcelVAlign.VAlignTop;

                //Thông tin khách hàng
                worksheet.Range["A7"].Text = "  THÔNG TIN KHÁCH HÀNG";
                worksheet.Range["A7"].CellStyle.Color = Color.FromArgb(0, 137, 71);
                worksheet.Range["A7"].CellStyle.Font.Bold = true;
                worksheet.Range["A7"].CellStyle.Font.Color = ExcelKnownColors.White;
                worksheet.Range["A7"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignLeft;
                worksheet.Range["A7"].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;

                worksheet.Range["A8"].Text = "Phan Tấn Đạt";
                worksheet.Range["A9"].Text = "Tường Lộc";
                worksheet.Range["A10"].Text = "Tam Bình";
                worksheet.Range["A11"].Text = "Vĩnh Long";
                worksheet.Range["A12"].Text = "0123456789";

                //Email
                IHyperLink hyperlink = worksheet.HyperLinks.Add(worksheet.Range["A13"]);
                hyperlink.Type = ExcelHyperLinkType.Url;
                hyperlink.Address = "phantandat97@gmail.com";
                hyperlink.ScreenTip = "Gửi mail";

                worksheet.Range["A15:B15"].Merge();
                worksheet.Range["A16:B16"].Merge();
                worksheet.Range["A17:B17"].Merge();
                worksheet.Range["A18:B18"].Merge();
                worksheet.Range["A19:B19"].Merge();
                worksheet.Range["A20:B20"].Merge();
                worksheet.Range["A21:B21"].Merge();
                worksheet.Range["A22:B22"].Merge();

                //Thông tin chi tiết đơn hàng
                worksheet.Range["A15"].Text = "  TÊN SẢN PHẨM";
                worksheet.Range["C15"].Text = "SL";
                worksheet.Range["D15"].Text = "ĐƠN GIÁ";
                worksheet.Range["E15"].Text = "TỔNG";
                worksheet.Range["A16"].Text = "Sản phẩm 1";
                worksheet.Range["A17"].Text = "Sản phẩm 2";
                worksheet.Range["A18"].Text = "Sản phẩm 3";
                worksheet.Range["A19"].Text = "Sản phẩm 4";
                worksheet.Range["A20"].Text = "Sản phẩm 5";
                worksheet.Range["C16"].Number = 3;
                worksheet.Range["C17"].Number = 2;
                worksheet.Range["C18"].Number = 1;
                worksheet.Range["C19"].Number = 4;
                worksheet.Range["C20"].Number = 3;
                worksheet.Range["D16"].Number = 21;
                worksheet.Range["D17"].Number = 54;
                worksheet.Range["D18"].Number = 10;
                worksheet.Range["D19"].Number = 20;
                worksheet.Range["D20"].Number = 30;
                worksheet.Range["D23"].Text = "Tổng thanh toán";

                //Format đơn vị tiền
                worksheet.Range["D16:E22"].NumberFormat = "0.00vnd";
                worksheet.Range["E23"].NumberFormat = "0.00vnd";

                //M đổ tổng tiền từ csdl vào hay dùng excel để tính, cái này là dùng công thức excel để tính
                application.EnableIncrementalFormula = true;
                worksheet.Range["E16:E20"].Formula = "=C16*D16";
                worksheet.Range["E23"].Formula = "=SUM(E16:E22)";

                //Format borders
                worksheet.Range["A16:E22"].CellStyle.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                worksheet.Range["A16:E22"].CellStyle.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                worksheet.Range["A16:E22"].CellStyle.Borders[ExcelBordersIndex.EdgeTop].Color = ExcelKnownColors.Grey_25_percent;
                worksheet.Range["A16:E22"].CellStyle.Borders[ExcelBordersIndex.EdgeBottom].Color = ExcelKnownColors.Grey_25_percent;
                worksheet.Range["A23:E23"].CellStyle.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                worksheet.Range["A23:E23"].CellStyle.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                worksheet.Range["A23:E23"].CellStyle.Borders[ExcelBordersIndex.EdgeTop].Color = ExcelKnownColors.Black;
                worksheet.Range["A23:E23"].CellStyle.Borders[ExcelBordersIndex.EdgeBottom].Color = ExcelKnownColors.Black;

                //Format font
                worksheet.Range["A3:E23"].CellStyle.Font.FontName = "Arial";
                worksheet.Range["A3:E23"].CellStyle.Font.Size = 10;
                worksheet.Range["A15:E15"].CellStyle.Font.Color = ExcelKnownColors.White;
                worksheet.Range["A15:E15"].CellStyle.Font.Bold = true;
                worksheet.Range["D23:E23"].CellStyle.Font.Bold = true;

                worksheet.Range["A15:E15"].CellStyle.Color = Color.FromArgb(0, 137, 71);

                worksheet.Range["A15"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignLeft;
                worksheet.Range["C15:C22"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                worksheet.Range["D15:E15"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;

                worksheet.Range["A1"].ColumnWidth = 36;
                worksheet.Range["B1"].ColumnWidth = 11;
                worksheet.Range["C1"].ColumnWidth = 8;
                worksheet.Range["D1:E1"].ColumnWidth = 18;
                worksheet.Range["A1"].RowHeight = 47;
                worksheet.Range["A2"].RowHeight = 15;
                worksheet.Range["A3:A4"].RowHeight = 15;
                worksheet.Range["A5"].RowHeight = 18;
                worksheet.Range["A6"].RowHeight = 29;
                worksheet.Range["A7"].RowHeight = 18;
                worksheet.Range["A8"].RowHeight = 15;
                worksheet.Range["A9:A14"].RowHeight = 15;
                worksheet.Range["A15:A23"].RowHeight = 18;


                //Saving the Excel to the MemoryStream 
                MemoryStream stream = new MemoryStream();

                workbook.SaveAs(stream);

                //Set the position as '0'.
                stream.Position = 0;

                //Download the Excel file in the browser
                FileStreamResult fileStreamResult = new FileStreamResult(stream, "application/excel");

                fileStreamResult.FileDownloadName = "HoaDon.xlsx";

                return fileStreamResult;
            }
        }
    }
}
