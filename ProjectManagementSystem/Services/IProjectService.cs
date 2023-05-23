﻿using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectManagementSystem.Data;
using ProjectManagementSystem.Models;
using System.Xml.Linq;
using ProjectManagementSystem.Controllers;
using Project = ProjectManagementSystem.Models.Project;
using ProjectManagementSystem.ViewModels.Project;

namespace ProjectManagementSystem.Services
{
    public interface IProjectService
    {
        Task<IEnumerable<Project>> GetAllProjectsAsync();
        Task<IEnumerable<Project>> GetAllProjectsByUserAsync(string email);
        Task<Project> GetProjectsAsync(Guid projectsId);
        Task<Guid> AddProjectAsync(ProjectTeamViewModel projects);
        Task<Guid> EditProjectAsync(ProjectTeamViewModel projects);
        Task DeleteProjectAsync(Guid projectsId);
        Dictionary<short, string> FillingKII();
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

        public async Task<Guid> AddProjectAsync(ProjectTeamViewModel model)
        {
            var project = new Project()
            {
                Id = Guid.NewGuid(),
                Name = model.Name,
                Description = model.Description,
                GitUrl = model.GitUrl,
                DateEnd = model.DateEnd,
                Attestation = model.Attestation,
                ProjectOwnerId = model.ProjectOwnerId,
                ProjectOwnerName = model.ProjectOwnerName
            };

            await _db.Projects.AddAsync(project);
            await _db.SaveChangesAsync();

            return project.Id;
        }

        public async Task<Guid> EditProjectAsync(ProjectTeamViewModel model)
        {
            var item = await _db.Projects.FirstOrDefaultAsync(s => s.Id == model.Id);
            item.Name = model.Name;
            item.Description = model.Description;
            item.GitUrl = model.GitUrl;
            item.DateEnd = model.DateEnd;
            item.Attestation = model.Attestation;
            await _db.SaveChangesAsync();

            return model.Id;
        }

        public async Task DeleteProjectAsync(Guid projectId)
        {
            var projects = await GetProjectsAsync(projectId);
            if (projects != null)
                _db.Projects.Remove(projects);
            await _db.SaveChangesAsync();
        }

        public Dictionary<short, string> FillingKII()
        {
            Dictionary<short, string> dict = new Dictionary<short, string>
            {
                { 0, "Нет" },
                { 1, "Да" }
            };
            return dict;
        }
    }
}