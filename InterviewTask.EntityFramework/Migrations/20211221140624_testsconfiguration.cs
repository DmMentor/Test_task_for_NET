using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InterviewTask.EntityFramework.Migrations
{
    public partial class testsconfiguration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "StartedParse",
                table: "Tests",
                type: "datetime2(0)",
                precision: 0,
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "StartedParse",
                table: "Tests",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2(0)",
                oldPrecision: 0,
                oldDefaultValueSql: "GETUTCDATE()");
        }
    }
}
