using core2.Entities;
using core2.Model;
using core2.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace core2.Controllers
{
    public class EmployeeController : Controller
    {
        private ICalculate _calculator;

        public EmployeeController(ICalculate calculator)
        {
            _calculator = calculator;
        }

        public IActionResult Add()
        {

            var employeeAddViewModel = new EmployeeAddViewModel
            {
                Employee = new Employee(),
                Cities = new List<SelectListItem>
                {
                   new SelectListItem{Text="Ankara",Value="6" },
                   new SelectListItem{Text="İstanbul",Value="34"}
                }
            };

            return View(employeeAddViewModel);
        }

        [HttpPost]
        public IActionResult Add(Employee employee)
        {

            return View();
        }
        public string Calculate()
        {

            return _calculator.Calculate(100).ToString();

        }
    }
}