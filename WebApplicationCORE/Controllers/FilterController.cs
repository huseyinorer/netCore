using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using core2.Filters;

namespace core2.Controllers
{
    public class FilterController : Controller
    {
        [CustomFilter]
        public IActionResult Index()
        {
            return View();
        }
    }
}