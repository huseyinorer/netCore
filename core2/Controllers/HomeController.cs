using core2.Entities;
using core2.Filters;
using core2.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;

namespace core2.Controllers
{
    public class HomeController : Controller
    {
        private readonly SchoolContext _school;
        public HomeController(SchoolContext school)
        {
            _school = school;

        }
        [Route("{id}")]
        public string Index()
        {
            //var x = _school.Database.ProviderName;
            //_school.persons.Add(new persons { personid = 3, lastname = "husam2233", firstname = "o223rer", address = "ev232", city = "kayser2233i" });
            //_school .SaveChanges();

            // var kayıt = _school.persons.Where(p => p.personid == 3).FirstOrDefault().lastname.ToString();
            return "Hello mı ??";
        }
        [HandleException(ViewName = "DivideByZeroError", exceptionType = typeof(DivideByZeroException))]
        [HandleException(ViewName = "Error", exceptionType = typeof(SecurityException))]
        public ViewResult Index2()
        {
            throw new SecurityException();
            return View();
        }
        public ViewResult Index3()
        {
            List<Employee> employees = new List<Employee>
            {
                new Employee{Id=1,FirstName="husam",LastName="orr",CityId=1},
                new Employee{Id=2,FirstName="husam2",LastName="orr2",CityId=6},
                new Employee{Id=3,FirstName="husam3",LastName="orr3",CityId=23},
            };
            List<string> cities = new List<string> { "ist", "ank", "maraş" };

            var model = new EmployeeListViewModel
            {
                Employees = employees,
                Cities = cities
            };
            return View(model);
        }

        public IActionResult Index4()
        {
            return StatusCode(200);
        }
        public IActionResult Index5()
        {
            return StatusCode(400);
        }
        public RedirectResult Index6()
        {
            return Redirect("/Home/Index2");
        }
        public IActionResult Index7()
        {
            return RedirectToAction("Index2");
        }

        public JsonResult Index9()
        {
            List<Employee> employees = new List<Employee>
            {
                new Employee{Id=1,FirstName="husam",LastName="orr",CityId=1},
                new Employee{Id=2,FirstName="husam2",LastName="orr2",CityId=6},
                new Employee{Id=3,FirstName="husam3",LastName="orr3",CityId=23},
            };

            return Json(employees);
        }

        public IActionResult RazerDemo()
        {
            List<Employee> employees = new List<Employee>
            {
                new Employee{Id=1,FirstName="husam",LastName="orr",CityId=1},
                new Employee{Id=2,FirstName="husam2",LastName="orr2",CityId=6},
                new Employee{Id=3,FirstName="husam3",LastName="orr3",CityId=23},
            };
            List<string> cities = new List<string> { "ist", "ank", "maraş" };

            var model = new EmployeeListViewModel
            {
                Employees = employees,
                Cities = cities
            };

            return View(model);

        }

        public JsonResult Index10(string key)
        {
            List<Employee> employees = new List<Employee>
            {
                new Employee{Id=1,FirstName="husam",LastName="orr",CityId=1},
                new Employee{Id=2,FirstName="husam2",LastName="orr2",CityId=6},
                new Employee{Id=3,FirstName="husam3",LastName="orr3",CityId=23},
            };
            if (string.IsNullOrEmpty(key))
            {
                return Json(employees);

            }
            var result = employees.Where(e => e.FirstName.ToLower().Contains(key));

            return Json(result);
        }

        public ViewResult EmployeeForm()
        {

            return View();
        }

    }
}