using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using ProjectManagementSystem.Models;
using ProjectManagementSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProjectManagementSystem.ViewModels.Admin;
using System.IO;
using System.Drawing;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace ProjectManagementSystem.Controllers
{
    [Authorize(Roles = "Администратор")]
    public class Admin : Controller
    {
        private readonly IUserService _userService;

        public Admin(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userService.GetAllUsersAsync();
            var model = new HomeViewModel()
            {
                Users = users,
            };
            return View(model);
        }

        public async Task<IActionResult> UpdateUser(Guid userId)
        {
            ViewBag.Roles = new SelectList(await _userService.GetAllRoleAsync(), "Id", "Name");
            return View(await _userService.GetUserAsync(userId));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUser(User model)
        {
            await _userService.EditUserAsync(model);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(User user)
        {
            await _userService.DeleteUserAsync(user.Id);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> ClearData()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
    }
}