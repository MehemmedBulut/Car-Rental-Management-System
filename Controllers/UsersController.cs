﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentalFinal.DAL;
using RentalFinal.Helpers;
using RentalFinal.Models;
using RentalFinal.ViewModels;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace RentalFinal.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _db;

        public UsersController(UserManager<AppUser> userManager, AppDbContext db)
        {
            _userManager = userManager;
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            List<AppUser> users = await _userManager.Users.ToListAsync();
            List<UserVM> userVMs = new List<UserVM>();
            foreach (AppUser user in users)
            {
                UserVM userVM = new UserVM()
                {
                    Id = user.Id,
                    Name = user.Name,
                    Surname = user.Surname,
                    Email = user.Email,
                    Username = user.UserName,
                    IsDeactive = user.IsDeactive,
                    Role = (await _userManager.GetRolesAsync(user))[0],
                };
                userVMs.Add(userVM);
            }
            return View(userVMs);
        }
        public IActionResult Create()
        {
            ViewBag.Roles = new List<string>()
            {
                Helper.Admin,
                Helper.ContentManager,
                Helper.Moderator
            };

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RegisterVM registerVM, string role)
        {
            ViewBag.Roles = new List<string>()
            {
                Helper.Admin,
                Helper.ContentManager,
                Helper.Moderator
            };
            AppUser appUser = new AppUser
            {
                Name = registerVM.Name,
                Email = registerVM.Email,
                Surname = registerVM.Surname,
                UserName = registerVM.Username,
            };
            IdentityResult ıdentityResult = await _userManager.CreateAsync(appUser, registerVM.Password);
            if (!ıdentityResult.Succeeded)
            {
                foreach (IdentityError error in ıdentityResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(registerVM);
            }
            await _userManager.AddToRoleAsync(appUser, role);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Update(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            AppUser user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return BadRequest();
            }
            UpdateVM dbUpdateVM = new UpdateVM()
            {
                Name = user.Name,
                Email = user.Email,
                Surname = user.Surname,
                Username = user.UserName,
                Role = (await _userManager.GetRolesAsync(user))[0],
            };
            ViewBag.Roles = new List<string>()
            {
                Helper.Admin,
                Helper.ContentManager,
            };
            return View(dbUpdateVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(string id, UpdateVM updateVM, string role)
        {
            ViewBag.Roles = new List<string>()
            {
                Helper.Admin,
                Helper.ContentManager,
                Helper.Moderator
            };
            if (id == null)
            {
                return NotFound();
            }
            AppUser user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return BadRequest();
            }
            UpdateVM dupdateVM = new UpdateVM()
            {
                Name = user.Name,
                Username = user.UserName,
                Email = user.Email,
                Surname = user.Surname,
                Role = (await _userManager.GetRolesAsync(user))[0],
            };
            user.Email = dupdateVM.Email;
            user.Name = dupdateVM.Name;
            user.Surname = dupdateVM.Surname;
            user.UserName = dupdateVM.Username;
            IdentityResult addIdentityResult = await _userManager.AddToRoleAsync(user, role);
            if (!addIdentityResult.Succeeded)
            {
                foreach (IdentityError error in addIdentityResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(dupdateVM);
            }
            IdentityResult removeIdentityResult = await _userManager.RemoveFromRoleAsync(user, dupdateVM.Role);
            if (!removeIdentityResult.Succeeded)
            {
                foreach (IdentityError error in removeIdentityResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(dupdateVM);
            }
            await _userManager.UpdateAsync(user);
            return RedirectToAction("Index");
        }

        #region Aktiv
        public async Task<IActionResult> Activity(string id)    /*//deletin post metodu*/
        {
            if (id == null)
            {
                return NotFound();
            }
            AppUser? dbUser = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (dbUser == null)
            {
                return BadRequest();
            }
            if (dbUser.IsDeactive)
            {
                dbUser.IsDeactive = false;
            }
            else
            {
                dbUser.IsDeactive = true;
            }
            await _userManager.UpdateAsync(dbUser);
            return RedirectToAction("Index");
        }
        #endregion

        public async Task<IActionResult> ResetPassword(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            AppUser user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return BadRequest();
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(string id, ResetPasswordVM resetPasswordVM)
        {
            if (id == null)
            {
                return NotFound();
            }
            AppUser user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return BadRequest();
            }
            string token = await _userManager.GeneratePasswordResetTokenAsync(user);
            IdentityResult ıdentityResult = await _userManager.ResetPasswordAsync(user, token, resetPasswordVM.Password);
            if (!ıdentityResult.Succeeded)
            {
                foreach (IdentityError error in ıdentityResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View();
            }
            return RedirectToAction("Index");
        }
    }
}
