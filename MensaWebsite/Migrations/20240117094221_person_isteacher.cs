using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MensaWebsite.Migrations
{
    /// <inheritdoc />
    public partial class person_isteacher : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                name: "IsTeacher",
                table: "Persons");
        }
    }
}
