using Microsoft.AspNetCore.Mvc;

namespace core2.Controllers
{
    [Route("admin")]
    public class AdminController : Controller
    {
        [Route("")]
        [Route("~/save")]
        public string Save()
        {

            return "Saved";
        }
        [Route("del")]
        public string Delete()
        {

            return "Deleted";
        }
        [Route("up")]
        public string Update()
        {

            return "Updated";
        }
    }
}