using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using core2.EntitiesF;
using core2.Model;
//razor pages diye geçer eski aspnet'e benzer yapı. eğer controller yok ise route ile buraya bakar 
namespace core2.Pages.Studentp
{
    public class IndexModel : PageModel
    {
        private readonly SchoolContext _context;
        public IndexModel(SchoolContext context)
        {
            _context = context;
        }

        public List<Student> Students { get; set; }
        public void OnGet()
        {
            Students = _context.Students.ToList();
        }
        [BindProperty]
        public Student Student { get; set; }
        public IActionResult OnPost()
        {
            _context.Students.Add(Student);
            _context.SaveChanges();
            return RedirectToPage("/Studentp/Index");
        }
    }
}