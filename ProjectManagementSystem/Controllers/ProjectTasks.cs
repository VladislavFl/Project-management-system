using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystem.Services;
using System.Threading.Tasks;
using ProjectManagementSystem.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using System.IO;
using System.Security.Claims;
using System.Security.Principal;

namespace ProjectManagementSystem.Controllers
{
    public class ProjectTasks : Controller
    {
        private readonly IProjectTasksService _projectTasksService;
        private readonly IUserService _userService;

        public ProjectTasks(IProjectTasksService productService, IUserService userService)
        {
            _projectTasksService = productService;
            _userService = userService;
        }

        [Authorize(Roles = "Администратор, Пользователь, Владелец проекта")]
        public async Task<IActionResult> Index(Guid userId)
        {
            var tasks = await _projectTasksService.GetAllTaskByUserAsync(userId);
            return View(tasks);
        }

        [Authorize(Roles = "Администратор, Пользователь, Владелец проекта")]
        [HttpPost]
        public async Task<IActionResult> Index2(Guid userId)
        {
            return View(await _projectTasksService.GetAllTaskByUserAsync(userId));
        }

        [Authorize(Roles = "Администратор, Пользователь, Владелец проекта")]
        public async Task<IActionResult> TaskDetail(Guid productId)
        {
            ViewBag.Users = new SelectList(await _userService.GetUserForTaskAsync(), "Id", "Name");
            return View(await _projectTasksService.GetProjectTasksAsync(productId));
        }

        [Authorize(Roles = "Администратор, Владелец проекта")]
        public async Task<IActionResult> TaskCreate()
        {
            ViewBag.Users = new SelectList(await _userService.GetUserForTaskAsync(), "Id", "Name");
            return View();
        }

        [Authorize(Roles = "Администратор, Владелец проекта")]
        [HttpPost]
        public async Task<IActionResult> TaskCreate(Tasks tasks)
        {
            /*var validateSerialNumber = (await _projectTasksService.GetAllProductsAsync()).FirstOrDefault(x => x.SerialNumber == tasks.SerialNumber);
            if (validateSerialNumber != null)
            {
                ModelState.AddModelError("", "Такой серийный номер уже существует");
                ViewBag.Users = new SelectList(await _userService.GetUserForProductionAsync(), "Id", "Login");
                return View(tasks);
            }*/

            var productId = await _projectTasksService.AddTaskAsync(tasks);
            return RedirectToAction("TaskDetail", new { productId });
        }

        [Authorize(Roles = "Администратор, Владелец проекта")]
        public async Task<IActionResult> TaskUpdate(Guid productId)
        {
            ViewBag.Users = new SelectList(await _userService.GetUserForTaskAsync(), "Id", "Name");
            return View(await _projectTasksService.GetProjectTasksAsync(productId));
        }

        [Authorize(Roles = "Администратор, Владелец проекта")]
        [HttpPost]
        public async Task<IActionResult> TaskUpdate(Tasks tasks)
        {
            var productId = await _projectTasksService.EditTaskAsync(tasks);
            return RedirectToAction("TaskDetail", new { productId });
        }

        [Authorize(Roles = "Администратор, Владелец проекта")]
        [HttpPost]
        public async Task<IActionResult> TaskDelete(Tasks tasks)
        {
            await _projectTasksService.DeleteTaskAsync(tasks.Id);
            return RedirectToAction("Index");
        }
    }
}