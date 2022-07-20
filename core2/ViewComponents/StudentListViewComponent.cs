using core2.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using System.Linq;

namespace core2.ViewComponents
{
    public class StudentListViewComponent : ViewComponent
    {
        private SchoolContext _context;

        public StudentListViewComponent(SchoolContext context)
        {
            _context = context;
        }

        public ViewViewComponentResult Invoke(string filter)
        {
            filter = HttpContext.Request.Query["filter"];
            return View(new StudentListViewModel
            {

                Students = _context.Students.ToList()
            });
        }
    }
}
