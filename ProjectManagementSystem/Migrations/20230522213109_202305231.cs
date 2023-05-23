using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectManagementSystem.Migrations
{
    public partial class _202305231 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProjectOwnerId",
                table: "Projects",
                type: "uniqueidentifier",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProjectOwnerId",
                table: "Projects");
        }
    }
}
