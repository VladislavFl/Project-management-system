using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystem.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using ProjectManagementSystem.Models;
using ProjectManagementSystem.ViewModels.Project;

namespace ProjectManagementSystem.Controllers
{
    public class Project : Controller
    {
        private readonly IProjectService _projectService;
        private readonly IUserService _userService;
        private Guid _currentProjectId;

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
                var projects = await _projectService.GetAllProjectsAsync();
                return View(projects);
            }
            else
            {
                var userEmail = HttpContext.User.Identity?.Name!;
                var projects = await _projectService.GetAllProjectsByUserAsync(userEmail);
                return View(projects);
            }
        }

        [Authorize(Roles = "Администратор, Пользователь, Владелец проекта")]
        public async Task<IActionResult> ProjectDetail(Guid projectId)
        {
            ViewBag.Users = new SelectList(await _userService.GetUserForTaskAsync(), "Id", "Name");
            ViewBag.Team = new SelectList(await _userService.GetUserForAddingToProjectAsync(), "Id", "Name");
            _currentProjectId = projectId;
            return View(await _projectService.GetProjectsAsync(projectId));
        }

        [Authorize(Roles = "Администратор, Владелец проекта")]
        public async Task<IActionResult> ProjectCreate()
        {
            ViewBag.Users = new SelectList(await _userService.GetUserForTaskAsync(), "Id", "Name");
            ViewBag.Team = new SelectList(await _userService.GetUserForAddingToProjectAsync(), "Id", "Name");
            return View();
        }

        [Authorize(Roles = "Администратор, Владелец проекта")]
        [HttpPost]
        public async Task<IActionResult> ProjectCreate(ProjectTeamViewModel model)
        {
            string[] selectedValues = Request.Form["teamList"];
            var users = await _userService.GetAllUsersAsync();
            var userEmail = HttpContext.User.Identity?.Name!;
            model.ProjectOwnerId = users.FirstOrDefault(x => x.EmailAddress == userEmail).Id;
            model.ProjectOwnerName = users.FirstOrDefault(x => x.EmailAddress == userEmail).Name;
            var projectId = await _projectService.AddProjectAsync(model);
            
            // заполнение projectId у выбранных в выпадающем списке пользователей
            if (selectedValues != null)
            {
                foreach (var item in selectedValues)
                {
                    await _userService.EditUserForProjectAsync(Guid.Parse(item), projectId);
                }
            }
            return RedirectToAction("ProjectDetail", new { projectId });
        }

        [Authorize(Roles = "Администратор, Владелец проекта")]
        public async Task<IActionResult> ProjectUpdate(Guid projectId)
        {
            ViewBag.Users = new SelectList(await _userService.GetUserForTaskAsync(), "Id", "Name");
            ViewBag.Team = new SelectList(await _userService.GetUserForAddingToProjectAsync(), "Id", "Name");
            return View(await _projectService.GetProjectsAsync(_currentProjectId));
        }

        [Authorize(Roles = "Администратор, Владелец проекта")]
        [HttpPost]
        public async Task<IActionResult> ProjectUpdate(ProjectTeamViewModel model)
        {
            var projectId = await _projectService.EditProjectAsync(model);
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