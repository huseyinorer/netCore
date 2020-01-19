using MainProject.Identity;
using MainProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MainProject.Controllers
{
    public class BaseController : Controller
    {
        protected UserManager<AppIdentityUser> _userManager { get; set; }
        protected SignInManager<AppIdentityUser> _signInManager { get; set; }
        protected RoleManager<AppIdentityRole> _roleManager { get; set; }
        protected ProjectDbContext _DbContext { get; set; }

        protected AppIdentityUser CurrentUser => _userManager.FindByNameAsync(User.Identity.Name).Result;
        public BaseController(UserManager<AppIdentityUser> userManager, SignInManager<AppIdentityUser> signInManager, RoleManager<AppIdentityRole> roleManager = null, ProjectDbContext DbContext = null)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _DbContext = DbContext;
        }

        public void AddModelError(IdentityResult result)
        {
            foreach (var item in result.Errors)
            {
                ModelState.AddModelError("", item.Description);
            }

        }
    }
}