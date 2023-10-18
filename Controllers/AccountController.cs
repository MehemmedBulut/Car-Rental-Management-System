using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RentalFinal.DAL;
using RentalFinal.Helpers;
using RentalFinal.Models;
using RentalFinal.ViewModels;
using System.Linq;
using System.Threading.Tasks;

namespace RentalFinal.Controllers
{
    
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }

        #region Giriş
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            AppUser appUser = await _userManager.FindByNameAsync(loginVM.Username);

            if (appUser == null)
            {
                appUser = await _userManager.FindByEmailAsync(loginVM.Username);
                if (appUser == null)
                {
                    ModelState.AddModelError("Username", "Bu adda Istifadəçi adı və ya email tapılmadı!");
                    return View();
                }
            }
            if (appUser.IsDeactive)
            {
                ModelState.AddModelError("Username", "Bu istifadəçi deaktivdir!");
                return View();
            }
            Microsoft.AspNetCore.Identity.SignInResult signInResult = await _signInManager.PasswordSignInAsync(appUser, loginVM.Password, loginVM.IsRemember, true);
            if (signInResult.IsLockedOut)
            {
                ModelState.AddModelError("", "1 dəqiqə sonra yenidən yoxlayın");
                return View();
            }
            if (!signInResult.Succeeded)
            {

                ModelState.AddModelError("Password", "Şifrə yalnışdır!");
                return View();
            }
            return RedirectToAction("Index", "Home");
        }
        #endregion
        #region Qeydiyyat
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Register()
        {

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
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

            if (_userManager.Users.Count() == 1)
            {
                await _userManager.AddToRoleAsync(appUser, Helper.Admin);

            }
            else
            {
                await _userManager.AddToRoleAsync(appUser, Helper.ContentManager);
            }
            await _signInManager.SignInAsync(appUser, registerVM.IsRemember);
            return RedirectToAction("Index", "Home");
        }
        #endregion
        #region Çıxış
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
        #endregion
        #region ForgotPassword
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordVM forgotPasswordVM)
        {
            AppUser user = await _userManager.FindByEmailAsync(forgotPasswordVM.Email);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "bele email yoxdu");
                return View();
            }
            string token = await _userManager.GeneratePasswordResetTokenAsync(user);


            string callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, Token = token }, HttpContext.Request.Scheme, HttpContext.Request.Host.Value);

            string body = $"Zəhmət olmasa, aşağıdakı linkə klikləməklə parolunuzu sıfırlayın: <a href='{callbackUrl}'>Reset Password</a>";


            await Helper.SendMail(callbackUrl, forgotPasswordVM.Email);



            TempData["ConfirmationMessage"] = "Link uğurla göndərildi email-a.";
            return RedirectToAction(nameof(ForgotPassword));
        }
        #endregion
        #region ResetPassword
        public async Task<IActionResult> ResetPassword(string userId, string token)
        {
            if (userId == null || token == null)
            {
                return NotFound();
            }

            AppUser appUser = await _userManager.FindByIdAsync(userId);

            if (appUser == null)
            {
                return BadRequest();
            }

            ResetPasswordVM model = new ResetPasswordVM
            {
                Id = userId,
                Token = token
            };

            return View(model);
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
            return RedirectToAction("Login");
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> ResetPassword(string userId, string token, ResetPasswordVM resetPasswordVM)
        //{
        //    if (userId == null || token == null)
        //    {
        //        return NotFound();
        //    }

        //    if (!ModelState.IsValid)
        //    {
        //        return View(resetPasswordVM);
        //    }

        //    AppUser appUser = await _userManager.FindByIdAsync(userId);

        //    if (appUser == null)
        //    {
        //        return BadRequest();
        //    }

        //    IdentityResult identityResult = await _userManager.ResetPasswordAsync(appUser, token, resetPasswordVM.Password);

        //    if (!identityResult.Succeeded)
        //    {
        //        foreach (IdentityError error in identityResult.Errors)
        //        {
        //            ModelState.AddModelError("", error.Description);
        //        }
        //        return View(resetPasswordVM);
        //    }

        //    return RedirectToAction("Login", "Account");
        //}
        #endregion
        #region RolYaratma
        //public async Task CreateRoles()
        //{
        //    if (!await _roleManager.RoleExistsAsync(Helper.Admin))
        //    {
        //        await _roleManager.CreateAsync(new IdentityRole { Name = Helper.Admin });
        //    }
        //    if (!await _roleManager.RoleExistsAsync(Helper.ContentManager))
        //    {
        //        await _roleManager.CreateAsync(new IdentityRole { Name = Helper.ContentManager });
        //    }
        //}
        #endregion

    }
}
