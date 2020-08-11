using MainProject.Helper;
using MainProject.Identity;
using MainProject.Models;
using MainProject.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MainProject.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(UserManager<AppIdentityUser> userManager, SignInManager<AppIdentityUser> signInManager, ProjectDbContext dbContext) : base(userManager, signInManager, null, dbContext)
        {
        }

        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Member");

            var homepagePhotos = _DbContext.HomeSliderPhotos.ToList();
            return View(homepagePhotos);
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

            if(_userManager.Users.Any(w=>w.PhoneNumber==registerViewModel.PhoneNumber))
            {
                ModelState.AddModelError("","Bu telefon numarası kayıtlıdır");
                return View(registerViewModel);

            }

            var user = new AppIdentityUser
            {
                UserName = registerViewModel.UserName,
                Email = registerViewModel.Email,
                PhoneNumber = registerViewModel.PhoneNumber,
            };

            var result = await _userManager.CreateAsync(user, registerViewModel.Password);

            if (result.Succeeded)
            {
                var confirmationCode = _userManager.GenerateEmailConfirmationTokenAsync(user);
                var callBackUrl = Url.Action("ConfirmEmail", "Home", new { userId = user.Id, code = confirmationCode.Result });

                //send email
                SendEmail.SendCallBackURL(user.Email, callBackUrl, _DbContext);

                return RedirectToAction("Login", "Home");
            }
            else
            {
                AddModelError(result);
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
                if (await _userManager.IsLockedOutAsync(user))
                {
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
                }
                else
                {
                    await _userManager.AccessFailedAsync(user);
                    var failed = await _userManager.GetAccessFailedCountAsync(user);
                    ModelState.AddModelError(string.Empty, $"{failed} kez başarısız giriş.");

                    if (failed == 3)
                    {
                        await _userManager.SetLockoutEndDateAsync(user, new DateTimeOffset(DateTime.Now.AddMinutes(3)));
                        ModelState.AddModelError(string.Empty, "Hesabınız 3 başarısız girişten dolayı 3 dakika kilitlenmiştir.");
                    }
                    else
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
            SendEmail.SendCallBackURL(email, callBackUrl, _DbContext);
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

        public IActionResult FacebookLogin(string ReturnUrl)
        {
            string RedirectUrl = Url.Action("ExternalResponse", "Home", new { ReturnUrl });
            var property = _signInManager.ConfigureExternalAuthenticationProperties("Facebook", RedirectUrl);

            return new ChallengeResult("Facebook", property);
        }

        public IActionResult GoogleLogin(string ReturnUrl)
        {
            string RedirectUrl = Url.Action("ExternalResponse", "Home", new { ReturnUrl });
            var property = _signInManager.ConfigureExternalAuthenticationProperties("Google", RedirectUrl);

            return new ChallengeResult("Google", property);
        }

        public async Task<IActionResult> ExternalResponse(string ReturnUrl = "/")
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return RedirectToAction("LogIn");
            }
            else
            {
                var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, true);
                // var user= await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);

                if (result.Succeeded)
                {
                    return Redirect(ReturnUrl);
                }
                else
                {
                    var userName = info.Principal.HasClaim(w => w.Type == ClaimTypes.Name)
                        ? info.Principal.FindFirst(ClaimTypes.Name).Value.Replace(' ', '-').ToLower() + info.Principal.FindFirst(ClaimTypes.NameIdentifier).Value.Substring(0, 5).ToString()
                        : info.Principal.FindFirst(ClaimTypes.Email).Value;

                    var newUser = new AppIdentityUser
                    {
                        Email = info.Principal.FindFirst(ClaimTypes.Email).Value,
                        Id = info.Principal.FindFirst(ClaimTypes.NameIdentifier).Value,
                        UserName = userName,
                        EmailConfirmed = true
                    };

                    var CreateResult = await _userManager.CreateAsync(newUser);
                    if (CreateResult.Succeeded)
                    {
                        var loginResult = await _userManager.AddLoginAsync(newUser, info);

                        if (loginResult.Succeeded)
                        {
                            await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, true);
                            return Redirect(ReturnUrl);
                        }
                        else
                        {
                            AddModelError(loginResult);
                        }
                    }
                    else
                    {
                        AddModelError(CreateResult);
                    }
                }
            }
            List<string> errors = ModelState.Values.SelectMany(x => x.Errors).Select(y => y.ErrorMessage).ToList();
            return View("Error", errors);
        }

        public ActionResult Error()
        {
            return View();
        }

        public IActionResult Policy()
        {
            return View();
        }
    }
}