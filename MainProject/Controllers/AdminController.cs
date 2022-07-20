using MainProject.Identity;
using MainProject.Models;
using MainProject.ViewModels;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MainProject.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : BaseController
    {
        public AdminController(UserManager<AppIdentityUser> userManager, RoleManager<AppIdentityRole> roleManager, ProjectDbContext dbcontext) : base(userManager, null, roleManager, dbcontext)
        {
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Roles()
        {
            return View(_roleManager.Roles.ToList());
        }

        public IActionResult RoleCreate()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RoleCreate(RoleViewModel model)
        {
            var role = new AppIdentityRole
            {
                Name = model.Name
            };

            var result = await _roleManager.CreateAsync(role);

            if (result.Succeeded)
            {
                return RedirectToAction("Roles");
            }
            else
            {
                AddModelError(result);
            }

            return View(model);
        }

        public IActionResult Users()
        {
            return View(_userManager.Users.ToList());
        }

        public IActionResult RoleDelete(string id)
        {
            var role = _roleManager.FindByIdAsync(id).Result;
            if (role != null)
            {
                var result = _roleManager.DeleteAsync(role).Result;
            }

            return RedirectToAction("Roles");
        }

        public IActionResult RoleUpdate(string id)
        {
            var role = _roleManager.FindByIdAsync(id).Result;

            if (role == null)
                return RedirectToAction("Roles");

            return View(role.Adapt<RoleViewModel>());
        }

        [HttpPost]
        public async Task<IActionResult> RoleUpdate(RoleViewModel model)
        {
            var role = await _roleManager.FindByIdAsync(model.Id);

            if (role != null)
            {
                role.Name = model.Name;
                var result = await _roleManager.UpdateAsync(role);

                if (result.Succeeded)
                    return RedirectToAction("Roles");
                else
                    AddModelError(result);
            }
            else
            {
                ModelState.AddModelError("", "Güncelleme Başarısız oldu.");
            }

            return View(model);
        }

        public IActionResult RoleAssign(string id)
        {
            TempData["UserId"] = id;
            var user = _userManager.FindByIdAsync(id).Result;
            ViewBag.userName = user.UserName;

            var roles = _roleManager.Roles;
            var userRoles = _userManager.GetRolesAsync(user).Result;

            var roleAssignViewModel = new List<RoleAssignViewModel>();

            foreach (var role in roles)
            {
                RoleAssignViewModel r = new RoleAssignViewModel();
                r.RoleId = role.Id;
                r.RoleName = role.Name;
                if (userRoles.Contains(role.Name))
                {
                    r.Exist = true;
                }
                else
                    r.Exist = false;

                roleAssignViewModel.Add(r);
            }

            return View(roleAssignViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> RoleAssign(List<RoleAssignViewModel> roleAssignViewModelList)
        {
            var user = await _userManager.FindByIdAsync(TempData["UserId"].ToString());
            if (user != null)
            {
                foreach (var item in roleAssignViewModelList)
                {
                    if (item.Exist)
                    {
                        await _userManager.AddToRoleAsync(user, item.RoleName);
                    }
                    else
                    {
                        await _userManager.RemoveFromRoleAsync(user, item.RoleName);
                    }
                }
            }
            return RedirectToAction("Users");
        }

        public IActionResult HomePagePhotos()
        {
            var homesliderphotos = _DbContext.HomeSliderPhotos.ToList();
            return View(homesliderphotos);
        }

        public IActionResult HomeSliderPhotoAdd()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> HomeSliderPhotoAdd(HomeSliderPhotoViewModel model, IFormFile Photo)
        {
            if (Photo.Length > 0)
            {
                using (var stream = new MemoryStream())
                {
                    await Photo.CopyToAsync(stream);
                    model.Photo = stream.ToArray();
                }

                var _photoid = _DbContext.HomeSliderPhotos.Max(w => w.PhotoId) + 1;
                _DbContext.HomeSliderPhotos.Add(new HomeSliderPhotos { Photo = model.Photo, PhotoId = _photoid, Description = model.Description, PhotoTitle = model.PhotoTitle, PhotoAddDate = DateTime.Now });
                _DbContext.SaveChanges();
            }

            return RedirectToAction("HomePagePhotos");
        }

        [HttpPost]
        public IActionResult DeleteHomeSliderPhoto(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var _id = Convert.ToInt32(id);
                var ExitingPhoto = _DbContext.HomeSliderPhotos.Where(w => w.PhotoId == _id).FirstOrDefault();

                _DbContext.HomeSliderPhotos.Remove(ExitingPhoto);
                _DbContext.SaveChanges();


            }
            return RedirectToAction("HomePagePhotos");
        }

        public IActionResult Claims()
        {
            return View(User.Claims.ToList());
        }
    }
}