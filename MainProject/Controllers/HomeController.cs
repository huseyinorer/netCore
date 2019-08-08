using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MainProject.Identity;
using MainProject.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MainProject.Controllers
{
    public class HomeController : Controller
    {
        public UserManager<AppIdentityUser> _userManager { get; set; }
        public SignInManager<AppIdentityUser> _signInManager { get; set; }

        public HomeController(UserManager<AppIdentityUser> userManager)
        {
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(RegisterUserViewModel registerViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(registerViewModel);
            }

            var user = new AppIdentityUser{
                UserName= registerViewModel.UserName,
                Email=registerViewModel.Email,
                PhoneNumber=registerViewModel.PhoneNumber,                
            };

            var result = await _userManager.CreateAsync(user, registerViewModel.Password);

            if(result.Succeeded)
            {
                var confirmationCode = _userManager.GenerateEmailConfirmationTokenAsync(user);
                var callBackUrl = Url.Action("ConfirmEmail", "Home", new { userId = user.Id, code = confirmationCode.Result });

                //send email

                return RedirectToAction("Login", "Home");

            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

            }

            return View(registerViewModel);
        }
        public IActionResult AccessDenied()//ders 65
        {
            return View();
        }

        public async Task<IActionResult> Logout()//ders 65
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult LogIn()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)//ders 64
        {
            if (!ModelState.IsValid)
            {
                return View(loginViewModel);
            }
            var user = await _userManager.FindByNameAsync(loginViewModel.UserName);
            if (user != null)
            {
                if (!await _userManager.IsEmailConfirmedAsync(user))
                {
                    ModelState.AddModelError(string.Empty, "Confirm your email");
                    return View(loginViewModel);
                }
            }

            var result = await _signInManager.PasswordSignInAsync(loginViewModel.UserName, loginViewModel.Password, false, false);
            if (result.Succeeded)
                return RedirectToAction("Index", "Home");

            ModelState.AddModelError(string.Empty, "Login failed");
            return View(loginViewModel);
        }

        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
                RedirectToAction("Index", "Home");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new ApplicationException("Unable to find the user");

            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
                return View("ConfirmEmail");

            return RedirectToAction("Index", "Home");
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            if (string.IsNullOrEmpty(email))
                return View();

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return View();

            var confirmationCode = await _userManager.GeneratePasswordResetTokenAsync(user);
            var callBackUrl = Url.Action("ResetPassword", "Security", new { userId = user.Id, code = confirmationCode });

            //send callback url with email

            return RedirectToAction("ForgotPasswordEmailSend");
        }

        public IActionResult ForgotPasswordEmailSend()
        {
            return View();
        }

        public IActionResult ResetPassword(string userId, string code)//ders 69
        {
            if (userId == null || code == null)
                throw new ApplicationException("Code must be supplied for password reset");

            var model = new ResetPasswordViewModel { Code = code };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel resetPasswordEmailModel)
        {
            if (!ModelState.IsValid)
                return View(resetPasswordEmailModel);

            var user = await _userManager.FindByEmailAsync(resetPasswordEmailModel.Email);
            if (user == null)
                throw new ApplicationException("User not found");

            var result = await _userManager.ResetPasswordAsync(user, resetPasswordEmailModel.Code, resetPasswordEmailModel.Password);

            if (result.Succeeded)
                return RedirectToAction("ResetPasswordConfirm");

            return View();
        }

        public IActionResult ResetPasswordConfirm()
        {
            return View();
        }
    }
}