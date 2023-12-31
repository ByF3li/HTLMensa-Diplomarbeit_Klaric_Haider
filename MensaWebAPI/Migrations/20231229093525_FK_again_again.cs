using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MensaWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class FK_again_again : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MenuPersons_Menues_menuId",
                table: "MenuPersons");

            migrationBuilder.DropForeignKey(
                name: "FK_MenuPersons_Persons_email",
                table: "MenuPersons");

            migrationBuilder.RenameColumn(
                name: "menuId",
                table: "MenuPersons",
                newName: "MenuId");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "MenuPersons",
                newName: "PersonEmail");

            migrationBuilder.RenameIndex(
                name: "IX_MenuPersons_menuId",
                table: "MenuPersons",
                newName: "IX_MenuPersons_MenuId");

            migrationBuilder.RenameIndex(
                name: "IX_MenuPersons_email",
                table: "MenuPersons",
                newName: "IX_MenuPersons_PersonEmail");

            migrationBuilder.AddForeignKey(
                name: "FK_MenuPersons_Menues_MenuId",
                table: "MenuPersons",
                column: "MenuId",
                principalTable: "Menues",
                principalColumn: "MenuId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MenuPersons_Persons_PersonEmail",
                table: "MenuPersons",
                column: "PersonEmail",
                principalTable: "Persons",
                principalColumn: "Email");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MenuPersons_Menues_MenuId",
                table: "MenuPersons");

            migrationBuilder.DropForeignKey(
                name: "FK_MenuPersons_Persons_PersonEmail",
                table: "MenuPersons");

            migrationBuilder.RenameColumn(
                name: "MenuId",
                table: "MenuPersons",
                newName: "menuId");

            migrationBuilder.RenameColumn(
                name: "PersonEmail",
                table: "MenuPersons",
                newName: "email");

            migrationBuilder.RenameIndex(
                name: "IX_MenuPersons_MenuId",
                table: "MenuPersons",
                newName: "IX_MenuPersons_menuId");

            migrationBuilder.RenameIndex(
                name: "IX_MenuPersons_PersonEmail",
                table: "MenuPersons",
                newName: "IX_MenuPersons_email");

            migrationBuilder.AddForeignKey(
                name: "FK_MenuPersons_Menues_menuId",
                table: "MenuPersons",
                column: "menuId",
                principalTable: "Menues",
                principalColumn: "MenuId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MenuPersons_Persons_email",
                table: "MenuPersons",
                column: "email",
                principalTable: "Persons",
                principalColumn: "Email");
        }
    }
}
