using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeleperformanceTask.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEmpolyeeModelUsersFieldUsername : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Employees",
                newName: "UserName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "Employees",
                newName: "Name");
        }
    }
}
