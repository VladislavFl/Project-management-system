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

        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Tasks> Tasks { get; set; }
        public DbSet<Models.Project> Projects { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var adminRole = new Role() { Id = Guid.Parse("3A1AED39-BCB4-4E00-846D-1481DEF6713F"), Name = "Администратор" };
            var managerRole = new Role() { Id = Guid.Parse("022DCB1C-D15E-474D-A125-823318A89F51"), Name = "Владелец проекта" };
            var userRole = new Role() { Id = Guid.Parse("2EACD188-1784-4C25-8742-9F6D8A36CB75"), Name = "Пользователь" };

            var adminUser = new User()
            {
                Id = Guid.Parse("166F4B58-F165-4A72-AB5B-B2406C80D751"),
                Name = "admin",
                EmailAddress = "admin@admin.com",
                Password = "admin",
                RoleId = adminRole.Id
            };

            modelBuilder.Entity<Role>().HasData(adminRole, managerRole, userRole);
            modelBuilder.Entity<User>().HasData(
                adminUser,
                new User()
                {
                    Id = Guid.Parse("66D42B36-6ACD-4B1F-8027-1B751392042C"),
                    Name = "vlad",
                    EmailAddress = "vlad@gmail.com",
                    Password = "vlad",
                    RoleId = userRole.Id
                },
                new User()
                {
                    Id = Guid.Parse("40471A5A-FFA5-44FD-BDD2-DF4BCB4249EB"),
                    Name = "manager",
                    EmailAddress = "manager@gmail.com",
                    Password = "manager",
                    RoleId = managerRole.Id
                }
                );

            base.OnModelCreating(modelBuilder);
        }
    }
}