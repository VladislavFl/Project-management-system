using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectManagementSystem.Data;
using ProjectManagementSystem.Models;
using ProjectManagementSystem.Controllers;

namespace ProjectManagementSystem.Services
{
    public interface IProjectTasksService
    {
        Task<IEnumerable<Tasks>> GetAllTasksAsync();
        Task<IEnumerable<Tasks>> GetAllTaskByUserAsync(string email);
        Task<Tasks> GetProjectTasksAsync(Guid tasksId);
        Task<Guid> AddTaskAsync(Tasks tasks, string projectId);
        Task<Guid> EditTaskAsync(Tasks tasks);
        Task DeleteTaskAsync(Guid tasksId);
        Dictionary<short, string> FillingPrioreties();
    }

    public class ProjectTasksService : IProjectTasksService
    {
        private readonly AppDbContext _db;

        public ProjectTasksService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Tasks>> GetAllTasksAsync()
        {
            return await _db.Tasks.Include(p => p.User)
                .AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<Tasks>> GetAllTaskByUserAsync(string email)
        {
            return await _db.Tasks.Include(p => p.User)
                .Where(p => p.User.EmailAddress == email)
                .AsNoTracking().ToListAsync();
        }

        public async Task<Tasks> GetProjectTasksAsync(Guid tasksId)
        {
            return await _db.Tasks.Include(p => p.User).AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == tasksId);
        }

        public async Task<Guid> AddTaskAsync(Tasks tasks, string projectId)
        {
            await _db.Tasks.AddAsync(tasks);
            var project = _db.Projects.Where(x => x.Id == Guid.Parse(projectId)).FirstOrDefault();
            if (project != null)
            {
                tasks.Project = project;
                await _db.SaveChangesAsync();
            }

            return tasks.Id;
        }

        public async Task<Guid> EditTaskAsync(Tasks tasks)
        {
            var item = await _db.Tasks.FirstOrDefaultAsync(s => s.Id == tasks.Id);
            item.Name = tasks.Name;
            item.Priorety = tasks.Priorety;
            item.Description = tasks.Description;
            item.UserId = tasks.UserId;
            await _db.SaveChangesAsync();

            return tasks.Id;
        }

        public async Task DeleteTaskAsync(Guid tasksId)
        {
            var tasks = await GetProjectTasksAsync(tasksId);
            if (tasks != null)
                _db.Tasks.Remove(tasks);
            await _db.SaveChangesAsync();
        }

        public Dictionary<short, string> FillingPrioreties()
        {
            Dictionary<short, string> dict = new Dictionary<short, string>
            {
                { 1, "Критически важно" },
                { 2, "Средний уровень важности" },
                { 3, "Низкий уровень важности" }
            };
            return dict;
        }
    }
}