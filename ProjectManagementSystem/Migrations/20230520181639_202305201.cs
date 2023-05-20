using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectManagementSystem.Migrations
{
    public partial class _202305201 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name" },
                values: new object[] { new Guid("022dcb1c-d15e-474d-a125-823318a89f51"), "Владелец проекта" });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name" },
                values: new object[] { new Guid("2eacd188-1784-4c25-8742-9f6d8a36cb75"), "Пользователь" });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name" },
                values: new object[] { new Guid("3a1aed39-bcb4-4e00-846d-1481def6713f"), "Администратор" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "EmailAddress", "Name", "Password", "RoleId" },
                values: new object[] { new Guid("166f4b58-f165-4a72-ab5b-b2406c80d751"), "admin@admin.com", "admin", "admin", new Guid("3a1aed39-bcb4-4e00-846d-1481def6713f") });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "EmailAddress", "Name", "Password", "RoleId" },
                values: new object[] { new Guid("40471a5a-ffa5-44fd-bdd2-df4bcb4249eb"), "manager@gmail.com", "manager", "manager", new Guid("022dcb1c-d15e-474d-a125-823318a89f51") });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "EmailAddress", "Name", "Password", "RoleId" },
                values: new object[] { new Guid("66d42b36-6acd-4b1f-8027-1b751392042c"), "vlad@gmail.com", "vlad", "vlad", new Guid("2eacd188-1784-4c25-8742-9f6d8a36cb75") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("166f4b58-f165-4a72-ab5b-b2406c80d751"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("40471a5a-ffa5-44fd-bdd2-df4bcb4249eb"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("66d42b36-6acd-4b1f-8027-1b751392042c"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("022dcb1c-d15e-474d-a125-823318a89f51"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("2eacd188-1784-4c25-8742-9f6d8a36cb75"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("3a1aed39-bcb4-4e00-846d-1481def6713f"));
        }
    }
}
