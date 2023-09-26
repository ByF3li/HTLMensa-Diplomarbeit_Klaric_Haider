using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MensaWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class ChangePriceDecimal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Menues",
                type: "decimal(5,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(2,2)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Menues",
                type: "decimal(2,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)");
        }
    }
}
