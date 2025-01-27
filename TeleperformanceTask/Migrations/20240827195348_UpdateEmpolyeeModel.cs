using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeleperformanceTask.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEmpolyeeModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "Employees",
                newName: "ImagePath");

            migrationBuilder.RenameColumn(
                name: "GradUndergrad",
                table: "Employees",
                newName: "EducationLevel");

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Role",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "ImagePath",
                table: "Employees",
                newName: "ImageUrl");

            migrationBuilder.RenameColumn(
                name: "EducationLevel",
                table: "Employees",
                newName: "GradUndergrad");
        }
    }
}
