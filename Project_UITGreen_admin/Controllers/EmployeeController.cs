using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Project_UITGreen_admin.Models;


namespace Project_UITGreen_admin.Controllers
{
    public class EmployeeController : Controller
    {
        public IActionResult Index()
    {
        List<Employee> listEmp = new List<Employee>();
        listEmp = Employee.SelectEmp();
        List<Image> listImg = new List<Image>();
        foreach (var item in listEmp)
        {
            listImg.Add(Image.SelectOne(item.id_img));
        }
        this.ViewBag.listImg = listImg;

        return View(listEmp);
    }
    [HttpGet]
    [Route("searchemp")]
    public IActionResult SearchEmp()
    {
        string search = Request.Query["search"].ToString();
        List<Employee> listEmp = new List<Employee>();
        listEmp = Employee.SearchEmp(search);
        List<Image> listImg = new List<Image>();
        foreach (var item in listEmp)
        {
            listImg.Add(Image.SelectOne(item.id_img));
        }
        this.ViewBag.listImg = listImg;
        this.ViewBag.search = search;
        return View("Index", listEmp);
    }
    public IActionResult Insert()
    {
        return View();
    }
    public IActionResult InsertEmp(Image img, Employee emp)
    {
        Image.InsertImg(img);
        Image image = Image.SelectNew();
        Employee.InsertEmp(emp, image.id_img);
        return RedirectToAction("Index");
    }
    public IActionResult Update(int id)
    {
        Employee emp = Employee.FindEmpByID(id);
        this.ViewBag.emp = emp;
        Image img = Image.SelectOne(emp.id_img);
        this.ViewBag.img = img;
        return View();
    }
    public IActionResult UpdateEmp(Employee emp, string link)
    {
        Employee.UpdateEmp(emp, link);
        return RedirectToAction("Index");
    }
    public IActionResult Delete(int id)
    {
        Employee emp = Employee.FindEmpByID(id);
        int id_img = emp.id_img;
        Employee.DeleteEmp(id);
        Image.DeleteImg(id_img);
        return RedirectToAction("Index");
    }
}
}
