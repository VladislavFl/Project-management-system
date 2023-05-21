﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Data;
using ProjectManagementSystem.Models;
using ProjectManagementSystem.ViewModels.Account;

namespace ProjectManagementSystem.Services
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<IEnumerable<User>> GetUserForTaskAsync();
        Task<User> GetUserByLoginAsync(string login);
        Task<User> GetUserByLoginAndPasswordAsync(string login, string password);
        Task<User> GetUserAsync(Guid userId);
        Task<User> GetUserAsync(string userEmail);
        Task<Guid> AddUserAsync(User user);
        Task<Guid> AddUserFromFileAsync(User user);
        Task AddUserAsync(RegisterViewModel model);
        Task<Guid> EditUserAsync(User user);
        Task<IEnumerable<Role>> GetAllRoleAsync();
        Task<bool> CheckUserByLoginAsync(string login);
        Task<bool> CheckUserByLoginAndPasswordAsync(string login, string password);
        Task DeleteUserAsync(Guid userId);
        Task ClearDataBase();
    }

    public class UserService : IUserService
    {
        private readonly AppDbContext _db;

        public UserService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _db.Users.Include(u => u.Role).AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<User>> GetUserForTaskAsync()
        {
            return await _db.Users.Include(u => u.Role)
                .Where(u => u.Role.Name == "Пользователь")
                .AsNoTracking().ToListAsync();
        }

        public async Task<User> GetUserByLoginAsync(string login)
        {
            return await _db.Users.Include(u => u.Role)
                .AsNoTracking().FirstOrDefaultAsync(u => u.EmailAddress == login);
        }

        public async Task<User> GetUserByLoginAndPasswordAsync(string login, string password)
        {
            return await _db.Users.Include(u => u.Role)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.EmailAddress == login && u.Password == password);
        }

        public async Task<User> GetUserAsync(Guid userId)
        {
            return await _db.Users.Include(u => u.Role)
                .AsNoTracking().FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<User> GetUserAsync(string userEmail)
        {
            return await _db.Users.Include(u => u.Role)
                .AsNoTracking().FirstOrDefaultAsync(u => u.EmailAddress == userEmail);
        }

        public async Task<Guid> AddUserAsync(User user)
        {
            await _db.Users.AddAsync(user);
            await _db.SaveChangesAsync();

            return user.Id;
        }

        public async Task<Guid> AddUserFromFileAsync(User user)
        {
            await _db.Users.AddAsync(user);
            await _db.SaveChangesAsync();

            return user.Id;
        }

        public async Task AddUserAsync(RegisterViewModel model)
        {
            var user = new User()
            {
                Id = Guid.NewGuid(),
                Name = model.Name,
                EmailAddress = model.Email,
                Password = model.Password,
                Role = await GetRoleByNameAsync("Пользователь")
            };
            await _db.Users.AddAsync(user);
            await _db.SaveChangesAsync();
        }

        private async Task<Role> GetRoleByNameAsync(string name)
        {
            return await _db.Roles.FirstOrDefaultAsync(r => r.Name == name);
        }

        public async Task<Guid> EditUserAsync(User user)
        {
            _db.Users.Update(user);
            await _db.SaveChangesAsync();

            return user.Id;
        }

        public async Task<IEnumerable<Role>> GetAllRoleAsync()
        {
            return await _db.Roles.ToListAsync();
        }

        public async Task<bool> CheckUserByLoginAsync(string login)
        {
            return (await _db.Users.Include(u => u.Role).AsNoTracking()
                       .FirstOrDefaultAsync(u => u.EmailAddress == login)) != null;
        }

        public async Task<bool> CheckUserByLoginAndPasswordAsync(string login, string password)
        {
            return (await _db.Users.Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.EmailAddress == login && u.Password == password)) != null;
        }

        public async Task DeleteUserAsync(Guid userId)
        {
            var user = await GetUserAsync(userId);
            if (user != null)
                _db.Users.Remove(user);
            await _db.SaveChangesAsync();
        }

        public async Task ClearDataBase()
        {
            /*_db.Users.RemoveRange(_db.Users.Where(u => u.Id != Guid.Parse("166F4B58-F165-4A72-AB5B-B2406C80D751")));
            await _db.SaveChangesAsync();*/
        }
    }
}