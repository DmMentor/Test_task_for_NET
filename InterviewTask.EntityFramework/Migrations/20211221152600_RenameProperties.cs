using Microsoft.EntityFrameworkCore.Migrations;

namespace InterviewTask.EntityFramework.Migrations
{
    public partial class RenameProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StartedParse",
                table: "Tests",
                newName: "ParsingDate");

            migrationBuilder.RenameColumn(
                name: "isLinkFromSitemap",
                table: "Links",
                newName: "IsLinkFromSitemap");

            migrationBuilder.RenameColumn(
                name: "isLinkFromHtml",
                table: "Links",
                newName: "IsLinkFromHtml");

            migrationBuilder.AlterColumn<string>(
                name: "Url",
                table: "Links",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ParsingDate",
                table: "Tests",
                newName: "StartedParse");

            migrationBuilder.RenameColumn(
                name: "IsLinkFromSitemap",
                table: "Links",
                newName: "isLinkFromSitemap");

            migrationBuilder.RenameColumn(
                name: "IsLinkFromHtml",
                table: "Links",
                newName: "isLinkFromHtml");

            migrationBuilder.AlterColumn<string>(
                name: "Url",
                table: "Links",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
