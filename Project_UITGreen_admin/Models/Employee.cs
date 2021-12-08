using System.Collections.Generic;
using System.Linq;

namespace Project_UITGreen_admin.Models
{
    public class Employee
    {
        public int id_emp { set; get; }
        public string name_emp { set; get; }
        public int id_img { set; get; }
        public string position { set; get; }
        public string phone { set; get; }
        public string email { set; get; }

        // xử lý
        public static void InsertEmp(Employee emp, int id_img1)
        {
            using (var context = new DataContext())
            {
                context.Employee.Add(new Employee
                {
                    name_emp = emp.name_emp,
                    id_img = id_img1,
                    position = emp.position,
                    phone = emp.phone,
                    email = emp.email,
                });
                context.SaveChanges();
            }
        }
        public static List<Employee> SelectEmp()
        {
            List<Employee> listEmp = new List<Employee>();
            using (var context = new DataContext())
            {
                var a = context.Employee;
                var employee = a.ToList();
                foreach (var emp in employee)
                {
                    listEmp.Add(new Employee()
                    {
                        id_emp = emp.id_emp,
                        name_emp = emp.name_emp,
                        id_img = emp.id_img,
                        position = emp.position,
                        phone = emp.phone,
                        email = emp.email,
                    });
                }
            }
            return listEmp;
        }
        public static List<Employee> SearchEmp(string search)
        {
            List<Employee> listEmp = new List<Employee>();
            using (var context = new DataContext())
            {
                var a = context.Employee;
                var employee = a
                .Where(p => p.name_emp.Contains(search)).ToList();
                foreach (var emp in employee)
                {
                    listEmp.Add(new Employee()
                    {
                        id_emp = emp.id_emp,
                        name_emp = emp.name_emp,
                        id_img = emp.id_img,
                        position = emp.position,
                        phone = emp.phone,
                        email = emp.email,
                    });
                }
            }
            return listEmp;
        }

        public static void DeleteEmp(int id)
        {
            using (var context = new DataContext())
            {
                var a = context.Employee;
                var emp = (from p in a
                           where (p.id_emp == id)
                           select p).FirstOrDefault();
                if (emp != null)
                {
                    context.Remove(emp);
                    context.SaveChanges();
                }
            }
        }
        public static void UpdateEmp(Employee emp, string link)
        {
            using (var context = new DataContext())
            {
                var a = context.Employee;
                Employee employee = (from p in a
                                     where (p.id_emp == emp.id_emp)
                                     select p).FirstOrDefault();
                Image img = new Image();
                img.id_img = employee.id_img;
                img.link = link;
                Image.UpdateImg(img);
                if (employee != null)
                {
                    employee.name_emp = emp.name_emp;
                    //employee.id_img = emp.id_img;
                    employee.position = emp.position;
                    employee.phone = emp.phone;
                    employee.email = emp.email;
                    context.SaveChanges();
                }

            }
        }
        public static Employee FindEmpByID(int id)
        {
            using (var context = new DataContext())
            {
                var a = context.Employee;
                Employee emp = (from p in a
                                where (p.id_emp == id)
                                select p).FirstOrDefault();
                return emp;
            }
        }
    }
}
