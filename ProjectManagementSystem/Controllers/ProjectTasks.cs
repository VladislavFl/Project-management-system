using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystem.Services;
using ProjectManagementSystem.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

namespace ProjectManagementSystem.Controllers
{
    public class ProjectTasks : Controller
    {
        private readonly IProjectTasksService _projectTasksService;
        private readonly IUserService _userService;
        private static Guid _currentTaskId;

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
        public async Task<IActionResult> TaskDetail(Guid taskId)
        {
            ViewBag.Users = new SelectList(await _userService.GetUserForAddingToProjectAsync(), "Id", "Name");
            ViewBag.Priorety = new SelectList(_projectTasksService.FillingPrioreties(), "Key", "Value");

            var userEmail = HttpContext.User.Identity?.Name!;
            var user = await _userService.GetUserByLoginAsync(userEmail);
            ViewBag.Projects = new SelectList(await _userService.GetProjectForTaskAsync(user.Id), "ProjectOwnerId", "Name");

            _currentTaskId = taskId;

            return View(await _projectTasksService.GetProjectTasksAsync(taskId));
        }

        [Authorize(Roles = "Администратор, Владелец проекта")]
        public async Task<IActionResult> TaskCreate()
        {
            ViewBag.Users = new SelectList(await _userService.GetUserForAddingToProjectAsync(), "Id", "Name");
            ViewBag.Priorety = new SelectList(_projectTasksService.FillingPrioreties(), "Key", "Value");

            var userEmail = HttpContext.User.Identity?.Name!;
            var user = await _userService.GetUserByLoginAsync(userEmail);
            ViewBag.Projects = new SelectList(await _userService.GetProjectForTaskAsync(user.Id), "Id", "Name");

            return View();
        }

        [Authorize(Roles = "Администратор, Владелец проекта")]
        [HttpPost]
        public async Task<IActionResult> TaskCreate(Tasks tasks)
        {
            string selectedValue = Request.Form["projectVal"];
            var productId = await _projectTasksService.AddTaskAsync(tasks, selectedValue);
            return RedirectToAction("Index", new { productId });
        }

        [Authorize(Roles = "Администратор, Владелец проекта")]
        public async Task<IActionResult> TaskUpdate()
        {
            ViewBag.Users = new SelectList(await _userService.GetUserForAddingToProjectAsync(), "Id", "Name");
            ViewBag.Priorety = new SelectList(_projectTasksService.FillingPrioreties(), "Key", "Value");

            var userEmail = HttpContext.User.Identity?.Name!;
            var user = await _userService.GetUserByLoginAsync(userEmail);
            ViewBag.Projects = new SelectList(await _userService.GetProjectForTaskAsync(user.Id), "ProjectOwnerId", "Name");

            return View(await _projectTasksService.GetProjectTasksAsync(_currentTaskId));
        }

        [Authorize(Roles = "Администратор, Владелец проекта")]
        [HttpPost]
        public async Task<IActionResult> TaskUpdate(Tasks tasks)
        {
            await _projectTasksService.EditTaskAsync(tasks);
            return RedirectToAction("Index", new { _currentTaskId });
        }

        [Authorize(Roles = "Администратор, Пользователь, Владелец проекта")]
        [HttpPost]
        public async Task<IActionResult> TaskDelete(Guid taskId)
        {
            await _projectTasksService.DeleteTaskAsync(taskId);
            return RedirectToAction("Index");
        }
    }
}