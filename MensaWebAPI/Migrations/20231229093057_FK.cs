using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MensaWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class FK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MenuPersons_Persons_email",
                table: "MenuPersons");

            migrationBuilder.UpdateData(
                table: "MenuPersons",
                keyColumn: "email",
                keyValue: null,
                column: "email",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "email",
                table: "MenuPersons",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "FK_MenuPersons_Persons_email",
                table: "MenuPersons",
                column: "email",
                principalTable: "Persons",
                principalColumn: "Email",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MenuPersons_Persons_email",
                table: "MenuPersons");

            migrationBuilder.AlterColumn<string>(
                name: "email",
                table: "MenuPersons",
                type: "varchar(255)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "FK_MenuPersons_Persons_email",
                table: "MenuPersons",
                column: "email",
                principalTable: "Persons",
                principalColumn: "Email");
        }
    }
}
