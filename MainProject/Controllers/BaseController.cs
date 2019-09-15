using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MainProject.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MainProject.Controllers
{
    public class BaseController : Controller
    {
        protected UserManager<AppIdentityUser> _userManager { get; set; }
        protected SignInManager<AppIdentityUser> _signInManager { get; set; }

        protected AppIdentityUser CurrentUser => _userManager.FindByNameAsync(User.Identity.Name).Result;
        public BaseController(UserManager<AppIdentityUser> userManager, SignInManager<AppIdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
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