using ProjectManagementSystem.Models;
using ProjectManagementSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ProjectManagementSystem.Controllers
{
    [Authorize(Roles = "Пользователь, Владелец проекта")]
    public class Profile : Controller
    {
        private readonly IUserService _userService;

        public Profile(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<IActionResult> UpdateUser()
        {
            var userEmail = HttpContext.User.Identity?.Name!;
            return View(await _userService.GetUserAsync(userEmail));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUser(User model)
        {
            await _userService.EditUserAsync(model);
            return RedirectToAction("OutInfoChangedProfile", "Profile");
        }

        public IActionResult OutInfoChangedProfile()
        {
            return View();
        }
    }
}