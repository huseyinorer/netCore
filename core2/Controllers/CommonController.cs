using Microsoft.AspNetCore.Mvc;

namespace core2.Controllers
{
    public class CommonController : Controller
    {
        [Route("/error")]
        public IActionResult Index()
        {
            return View();
        }
    }
}