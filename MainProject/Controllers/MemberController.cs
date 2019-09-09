using MainProject.Identity;
using MainProject.ViewModels;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MainProject.Controllers
{
    [Authorize]
    public class MemberController : Controller
    {
        public UserManager<AppIdentityUser> _userManager { get; set; }
        public SignInManager<AppIdentityUser> _signInManager { get; set; }

        public MemberController(UserManager<AppIdentityUser> userManager, SignInManager<AppIdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            var user = _userManager.FindByNameAsync(User.Identity.Name).Result;

            RegisterUserViewModel userViewModel = user.Adapt<RegisterUserViewModel>();

            return View(userViewModel);
        }

        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel changePasswordViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);

                bool currentPasswordValid = _userManager.CheckPasswordAsync(user, changePasswordViewModel.OldPassword).Result;

                if (currentPasswordValid)
                {
                    var result = _userManager.ChangePasswordAsync(user, changePasswordViewModel.OldPassword, changePasswordViewModel.NewPassword).Result;

                    if (result.Succeeded)
                    {
                        await _userManager.UpdateSecurityStampAsync(user);

                        //giriş-çıkış işlemini şifre değişir değişmez yapmak için
                        await _signInManager.SignOutAsync();
                        await _signInManager.PasswordSignInAsync(user, changePasswordViewModel.NewPassword, true, false);

                        ViewBag.Success = "true";
                    }
                    else
                    {
                        foreach (var item in result.Errors)
                        {
                            ModelState.AddModelError("", item.Description);
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Geçerli şifre yanlış!");
                }
            }

            return View(changePasswordViewModel);
        }

        public IActionResult UserInfoEdit()
        {
            var user = _userManager.FindByNameAsync(User.Identity.Name).Result;

            RegisterUserViewModel userViewModel = user.Adapt<RegisterUserViewModel>();

            return View(userViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> UserInfoEdit(RegisterUserViewModel registerUserViewModel)
        {
            ModelState.Remove("Password");
           if(ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                user.UserName = registerUserViewModel.UserName;
                user.Email = registerUserViewModel.Email;
                user.PhoneNumber = registerUserViewModel.PhoneNumber;

                var result=await _userManager.UpdateAsync(user);

                if(result.Succeeded)
                {
                    await _userManager.UpdateSecurityStampAsync(user);

                    //giriş-çıkış işlemini şifre değişir değişmez yapmak için
                    await _signInManager.SignOutAsync();
                    await _signInManager.SignInAsync(user, true);

                    ViewBag.Success = "true";

                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("",item.Description);
                    }
                }


            }

            return View(registerUserViewModel);
        }

        public void Logout()
        {
            _signInManager.SignOutAsync();
            

        }
    }
}