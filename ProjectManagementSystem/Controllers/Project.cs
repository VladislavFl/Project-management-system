using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystem.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using ProjectManagementSystem.Models;

namespace ProjectManagementSystem.Controllers
{
    public class Project : Controller
    {
        private readonly IProjectService _projectService;
        private readonly IUserService _userService;

        public Project(IProjectService projectService, IUserService userService)
        {
            _projectService = projectService;
            _userService = userService;
        }

        [Authorize(Roles = "Администратор, Пользователь, Владелец проекта")]
        public async Task<IActionResult> Index()
        {
            var role = User.Claims.FirstOrDefault(x => x.Value == "Пользователь");
            if (role == null)
            {
                var tasks = await _projectService.GetAllProjectsAsync();
                return View(tasks);
            }
            else
            {
                var userEmail = HttpContext.User.Identity?.Name!;
                var tasks = await _projectService.GetAllProjectsByUserAsync(userEmail);
                return View(tasks);
            }
        }

        [Authorize(Roles = "Администратор, Пользователь, Владелец проекта")]
        public async Task<IActionResult> ProjectDetail(Guid projectId)
        {
            ViewBag.Users = new SelectList(await _userService.GetUserForTaskAsync(), "Id", "Name");
            return View(await _projectService.GetProjectsAsync(projectId));
        }

        [Authorize(Roles = "Администратор, Владелец проекта")]
        public async Task<IActionResult> ProjectCreate()
        {
            ViewBag.Users = new SelectList(await _userService.GetUserForTaskAsync(), "Id", "Name");
            return View();
        }

        [Authorize(Roles = "Администратор, Владелец проекта")]
        [HttpPost]
        public async Task<IActionResult> ProjectCreate(Models.Project project)
        {
            var projectId = await _projectService.AddProjectAsync(project);
            return RedirectToAction("ProjectDetail", new { projectId });
        }

        [Authorize(Roles = "Администратор, Владелец проекта")]
        public async Task<IActionResult> ProjectUpdate(Guid projectId)
        {
            ViewBag.Users = new SelectList(await _userService.GetUserForTaskAsync(), "Id", "Name");
            return View(await _projectService.GetProjectsAsync(projectId));
        }

        [Authorize(Roles = "Администратор, Владелец проекта")]
        [HttpPost]
        public async Task<IActionResult> ProjectUpdate(Models.Project project)
        {
            var projectId = await _projectService.EditProjectAsync(project);
            return RedirectToAction("ProjectDetail", new { projectId });
        }

        [Authorize(Roles = "Администратор, Владелец проекта")]
        [HttpPost]
        public async Task<IActionResult> ProjectDelete(Models.Project project)
        {
            await _projectService.DeleteProjectAsync(project.Id);
            return RedirectToAction("Index");
        }
    }
}