﻿using MainProject.Enums;
using MainProject.Identity;
using MainProject.ViewModels;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Threading.Tasks;

namespace MainProject.Controllers
{
    [Authorize]
    public class MemberController : BaseController
    {
        public MemberController(UserManager<AppIdentityUser> userManager, SignInManager<AppIdentityUser> signInManager) : base(userManager, signInManager)
        {
        }

        public IActionResult Index()
        {
            var user = CurrentUser;

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
                var user = CurrentUser;

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
                        AddModelError(result);
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
            var user = CurrentUser;

            RegisterUserViewModel userViewModel = user.Adapt<RegisterUserViewModel>();
            ViewBag.Gender = new SelectList(Enum.GetNames(typeof(Gender)));
            return View(userViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> UserInfoEdit(RegisterUserViewModel registerUserViewModel)
        {
            ModelState.Remove("Password");
            ViewBag.Gender = new SelectList(Enum.GetNames(typeof(Gender)));

            if (ModelState.IsValid)
            {
                var user = CurrentUser;
                user.UserName = registerUserViewModel.UserName;
                user.Email = registerUserViewModel.Email;
                user.PhoneNumber = registerUserViewModel.PhoneNumber;
                user.BirthDate = registerUserViewModel.BirthDate;
                user.City = registerUserViewModel.City;
                user.Gender = (int)registerUserViewModel.Gender;
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
                    AddModelError(result);
                }


            }

            return View(registerUserViewModel);
        }

        public void Logout()
        {
            _signInManager.SignOutAsync();
            

        }
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}