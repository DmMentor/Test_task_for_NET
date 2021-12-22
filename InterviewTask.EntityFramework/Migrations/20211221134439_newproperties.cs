using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InterviewTask.EntityFramework.Migrations
{
    public partial class newproperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "StartedParse",
                table: "Tests",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "Tests",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StartedParse",
                table: "Tests");

            migrationBuilder.DropColumn(
                name: "Url",
                table: "Tests");
        }
    }
}
