using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MensaWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class mzunnnnnnnnnnnn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "InShoppingcart",
                table: "MenuPersons",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InShoppingcart",
                table: "MenuPersons");
        }
    }
}
