using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using MainProject.Helper;
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

        public HomeController(UserManager<AppIdentityUser> userManager,SignInManager<AppIdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Member");

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
                SendEmail.SendCallBackURL(user.Email, callBackUrl);

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

        public IActionResult LogIn(string ReturnUrl)
        {
            TempData["ReturnUrl"] = ReturnUrl;
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)//ders 64
        {
            if (!ModelState.IsValid)
            {
                return View(loginViewModel);
            }
            var user = await _userManager.FindByEmailAsync(loginViewModel.Email);
            if (user != null)
            {
                if(await _userManager.IsLockedOutAsync(user))                {

                    ModelState.AddModelError(string.Empty, "Hesabınız kısa süreliğine kilitlenmiştir. Lütfen biraz sonra tekrar deneyiniz.");
                    return View(loginViewModel);
                }
                if (!await _userManager.IsEmailConfirmedAsync(user))
                {
                    ModelState.AddModelError(string.Empty, "Confirm your email");
                    return View(loginViewModel);
                }

                await _signInManager.SignOutAsync();
                var result = await _signInManager.PasswordSignInAsync(user, loginViewModel.Password, loginViewModel.RememberMe, false);
                if (result.Succeeded)
                {
                    await _userManager.ResetAccessFailedCountAsync(user);

                    if (TempData["ReturnUrl"] != null)
                        return Redirect(TempData["ReturnUrl"].ToString());
                    return RedirectToAction("Index", "Member");
                }else
                {

                    await _userManager.AccessFailedAsync(user);
                    var failed = await _userManager.GetAccessFailedCountAsync(user);
                    ModelState.AddModelError(string.Empty, $"{failed} kez başarısız giriş.");

                    if (failed == 3)
                    {
                        await _userManager.SetLockoutEndDateAsync(user, new DateTimeOffset(DateTime.Now.AddMinutes(3)));
                        ModelState.AddModelError(string.Empty, "Hesabınız 3 başarısız girişten dolayı 3 dakika kilitlenmiştir.");
                    }else
                    {
                        ModelState.AddModelError(string.Empty, "Email adresi veya şifre yanlış.");

                    }
                }
            }
           else
            ModelState.AddModelError(string.Empty, "Email adresi ile kayıtlı kullanıcı bulunamamıştır.");
           
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
            var callBackUrl = Url.Action("ResetPassword", "Home", new { userId = user.Id, code = confirmationCode });

            //send callback url with email
            SendEmail.SendCallBackURL(email, callBackUrl);
            //

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

            TempData["userId"] = userId;
            TempData["code"] = code;
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
            {
                await _userManager.UpdateSecurityStampAsync(user);
                return RedirectToAction("ResetPasswordConfirm");
            }
            return View();
        }

        public IActionResult ResetPasswordConfirm()
        {           
            return View();
        }

    }
}