using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MensaWebsite.Migrations
{
    /// <inheritdoc />
    public partial class Persons_Update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Password",
                table: "Persons",
                newName: "Lastname");

            migrationBuilder.AddColumn<string>(
                name: "Firstname",
                table: "Persons",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<bool>(
                name: "IsTeacher",
                table: "Persons",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Firstname",
                table: "Persons");

            migrationBuilder.DropColumn(
                name: "IsTeacher",
                table: "Persons");

            migrationBuilder.RenameColumn(
                name: "Lastname",
                table: "Persons",
                newName: "Password");
        }
    }
}
