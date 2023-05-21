﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ProjectManagementSystem.Data;

#nullable disable

namespace ProjectManagementSystem.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20230521124044_202305211")]
    partial class _202305211
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.16")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("ProjectManagementSystem.Models.Project", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Attestation")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateEnd")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("GitUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("ProjectManagementSystem.Models.Role", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Roles");

                    b.HasData(
                        new
                        {
                            Id = new Guid("3a1aed39-bcb4-4e00-846d-1481def6713f"),
                            Name = "Администратор"
                        },
                        new
                        {
                            Id = new Guid("022dcb1c-d15e-474d-a125-823318a89f51"),
                            Name = "Владелец проекта"
                        },
                        new
                        {
                            Id = new Guid("2eacd188-1784-4c25-8742-9f6d8a36cb75"),
                            Name = "Пользователь"
                        });
                });

            modelBuilder.Entity("ProjectManagementSystem.Models.Tasks", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Priorety")
                        .HasColumnType("int");

                    b.Property<Guid?>("ProjectId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.HasIndex("UserId");

                    b.ToTable("Tasks");
                });

            modelBuilder.Entity("ProjectManagementSystem.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("EmailAddress")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("ProjectId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.HasIndex("RoleId");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = new Guid("166f4b58-f165-4a72-ab5b-b2406c80d751"),
                            EmailAddress = "admin@admin.com",
                            Name = "admin",
                            Password = "admin",
                            RoleId = new Guid("3a1aed39-bcb4-4e00-846d-1481def6713f")
                        },
                        new
                        {
                            Id = new Guid("66d42b36-6acd-4b1f-8027-1b751392042c"),
                            EmailAddress = "vlad@gmail.com",
                            Name = "vlad",
                            Password = "vlad",
                            RoleId = new Guid("2eacd188-1784-4c25-8742-9f6d8a36cb75")
                        },
                        new
                        {
                            Id = new Guid("40471a5a-ffa5-44fd-bdd2-df4bcb4249eb"),
                            EmailAddress = "manager@gmail.com",
                            Name = "manager",
                            Password = "manager",
                            RoleId = new Guid("022dcb1c-d15e-474d-a125-823318a89f51")
                        });
                });

            modelBuilder.Entity("ProjectManagementSystem.Models.Tasks", b =>
                {
                    b.HasOne("ProjectManagementSystem.Models.Project", "Project")
                        .WithMany()
                        .HasForeignKey("ProjectId");

                    b.HasOne("ProjectManagementSystem.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("Project");

                    b.Navigation("User");
                });

            modelBuilder.Entity("ProjectManagementSystem.Models.User", b =>
                {
                    b.HasOne("ProjectManagementSystem.Models.Project", null)
                        .WithMany("Users")
                        .HasForeignKey("ProjectId");

                    b.HasOne("ProjectManagementSystem.Models.Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("ProjectManagementSystem.Models.Project", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("ProjectManagementSystem.Models.Role", b =>
                {
                    b.Navigation("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
