using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectManagementSystem.Data;
using ProjectManagementSystem.Models;

namespace ProjectManagementSystem.Services
{
    public interface IProjectTasksService
    {
        Task<IEnumerable<Tasks>> GetAllTasksAsync();
        //Task<IEnumerable<Tasks>> GetAllProductForRepair();
        //Task<IEnumerable<Tasks>> GetAllProductsForShipmentAsync();
        Task<IEnumerable<Tasks>> GetAllTaskByUserAsync(string email);
        Task<Tasks> GetProjectTasksAsync(Guid tasksId);
        Task<Guid> AddTaskAsync(Tasks tasks);
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

        /*public async Task<IEnumerable<Tasks>> GetAllProductForRepair()
        {
            return await _db.Shipments.Select(s => s.Product)
                .OrderBy(p => p.CreatedDate)
                .AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<Tasks>> GetAllProductsForShipmentAsync()
        {
            var result = await _db.Tasks.Where(p => _db.Shipments.All(i => i.ProductId != p.Id))
                .AsNoTracking().ToListAsync();
            return result;
        }*/

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

        public async Task<Guid> AddTaskAsync(Tasks tasks)
        {
            await _db.Tasks.AddAsync(tasks);
            await _db.SaveChangesAsync();

            return tasks.Id;
        }

        public async Task<Guid> EditTaskAsync(Tasks tasks)
        {
            //_db.Products.Update(product);
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