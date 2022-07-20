using core2.Filters;
using Microsoft.AspNetCore.Mvc;

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