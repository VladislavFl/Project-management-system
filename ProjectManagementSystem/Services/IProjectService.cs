using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectManagementSystem.Data;
using ProjectManagementSystem.Models;
using System.Xml.Linq;
using ProjectManagementSystem.Controllers;
using Project = ProjectManagementSystem.Models.Project;

namespace ProjectManagementSystem.Services
{
    public interface IProjectService
    {
        Task<IEnumerable<Project>> GetAllProjectsAsync();
        Task<IEnumerable<Project>> GetAllProjectsByUserAsync(string email);
        Task<Project> GetProjectsAsync(Guid projectsId);
        Task<Guid> AddProjectAsync(Project projects);
        Task<Guid> EditProjectAsync(Project projects);
        Task DeleteProjectAsync(Guid projectsId);
    }

    public class ProjectService : IProjectService
    {
        private readonly AppDbContext _db;

        public ProjectService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Project>> GetAllProjectsAsync()
        {
            return await _db.Projects.Include(p => p.User)
                .AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<Project>> GetAllProjectsByUserAsync(string email)
        {
            return await _db.Projects.Include(p => p.User)
                .Where(p => p.User.EmailAddress == email)
                .AsNoTracking().ToListAsync();
        }

        public async Task<Project> GetProjectsAsync(Guid projectsId)
        {
            return await _db.Projects.Include(p => p.User).AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == projectsId);
        }

        public async Task<Guid> AddProjectAsync(Project projects)
        {
            await _db.Projects.AddAsync(projects);
            await _db.SaveChangesAsync();

            return projects.Id;
        }

        public async Task<Guid> EditProjectAsync(Project projects)
        {
            var item = await _db.Projects.FirstOrDefaultAsync(s => s.Id == projects.Id);
            item.Name = projects.Name;
            item.Users = projects.Users;
            item.Description = projects.Description;
            item.GitUrl = projects.GitUrl;
            item.DateEnd = projects.DateEnd;
            item.UserId = projects.UserId;
            await _db.SaveChangesAsync();

            return projects.Id;
        }

        public async Task DeleteProjectAsync(Guid projectId)
        {
            var projects = await GetProjectsAsync(projectId);
            if (projects != null)
                _db.Projects.Remove(projects);
            await _db.SaveChangesAsync();
        }
    }
}