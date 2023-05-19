using System;
using System.Data;
using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Controllers;
using ProjectManagementSystem.Models;

namespace ProjectManagementSystem.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Role>? Roles { get; set; }
        public DbSet<User>? Users { get; set; }
    }
}