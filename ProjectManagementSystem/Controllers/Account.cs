using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using ProjectManagementSystem.Models;
using ProjectManagementSystem.Services;
using ProjectManagementSystem.ViewModels.Account;

namespace ProjectManagementSystem.Controllers
{
    public class Account : Controller
    {
        private readonly IUserService _userService;

        public Account(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var user = await _userService.GetUserByLoginAsync(model.Email);

            if (user == null)
            {
                await _userService.AddUserAsync(model);

                return RedirectToAction("OutInfoRegisterUser", "Account");
            }

            ModelState.AddModelError("", "Такой пользователь уже существует");
            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var user = await _userService.GetUserByLoginAndPasswordAsync(model.Email, model.Password);
            if (user != null)
            {
                await Authenticate(user);

                return RedirectToAction("Index", user.Role.Name == "Администратор" ? "Admin" : "ProjectTasks");
            }
            ModelState.AddModelError("", "Неверный логин и (или) пароль");
            return View(model);
        }

        public IActionResult OutInfoRegisterUser()
        {
            return View();
        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login");
        }

        private async Task Authenticate(User user)
        {
            var claims = new List<Claim>
            {
                new(ClaimsIdentity.DefaultNameClaimType, user.EmailAddress),
                new(ClaimsIdentity.DefaultRoleClaimType, user.Role?.Name!)
            };

            var id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }
    }
}
