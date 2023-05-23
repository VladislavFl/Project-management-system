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
using System.Collections;

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
        public async Task<IActionResult> Index()
        {
            var role = User.Claims.FirstOrDefault(x => x.Value == "Пользователь");
            if (role == null)
            {
                var tasks = await _projectTasksService.GetAllTasksAsync();
                return View(tasks);
            }
            else
            {
                var userEmail = HttpContext.User.Identity?.Name!;
                var tasks = await _projectTasksService.GetAllTaskByUserAsync(userEmail);
                return View(tasks);
            }
        }

        [Authorize(Roles = "Администратор, Пользователь, Владелец проекта")]
        public async Task<IActionResult> TaskDetail(Guid productId)
        {
            ViewBag.Users = new SelectList(await _userService.GetUserForAddingToProjectAsync(), "Id", "Name");
            ViewBag.Priorety = new SelectList(_projectTasksService.FillingPrioreties(), "Key", "Value");

            var userEmail = HttpContext.User.Identity?.Name!;
            var user = await _userService.GetUserByLoginAsync(userEmail);
            ViewBag.Projects = new SelectList(await _userService.GetProjectForTaskAsync(user.Id), "ProjectOwnerId", "Name");

            return View(await _projectTasksService.GetProjectTasksAsync(productId));
        }

        [Authorize(Roles = "Администратор, Владелец проекта")]
        public async Task<IActionResult> TaskCreate()
        {
            ViewBag.Users = new SelectList(await _userService.GetUserForAddingToProjectAsync(), "Id", "Name");
            ViewBag.Priorety = new SelectList(_projectTasksService.FillingPrioreties(), "Key", "Value");

            var userEmail = HttpContext.User.Identity?.Name!;
            var user = await _userService.GetUserByLoginAsync(userEmail);
            ViewBag.Projects = new SelectList(await _userService.GetProjectForTaskAsync(user.Id), "ProjectOwnerId", "Name");

            return View();
        }

        [Authorize(Roles = "Администратор, Владелец проекта")]
        [HttpPost]
        public async Task<IActionResult> TaskCreate(Tasks tasks)
        {
            string selectedValue = Request.Form["projectVal"];
            var productId = await _projectTasksService.AddTaskAsync(tasks, selectedValue);
            return RedirectToAction("TaskDetail", new { productId });
        }

        [Authorize(Roles = "Администратор, Владелец проекта")]
        public async Task<IActionResult> TaskUpdate(Guid productId)
        {
            ViewBag.Users = new SelectList(await _userService.GetUserForAddingToProjectAsync(), "Id", "Name");
            ViewBag.Priorety = new SelectList(_projectTasksService.FillingPrioreties(), "Key", "Value");

            var userEmail = HttpContext.User.Identity?.Name!;
            var user = await _userService.GetUserByLoginAsync(userEmail);
            ViewBag.Projects = new SelectList(await _userService.GetProjectForTaskAsync(user.Id), "ProjectOwnerId", "Name");

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