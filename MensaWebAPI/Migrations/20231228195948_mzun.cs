using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MensaWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class mzun : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Menues",
                columns: table => new
                {
                    MenuId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    WhichMenu = table.Column<int>(type: "int", nullable: false),
                    Starter = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MainCourse = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Price = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Menues", x => x.MenuId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Persons",
                columns: table => new
                {
                    Email = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Password = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persons", x => x.Email);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "MenuPersons",
                columns: table => new
                {
                    MenuPersonId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    OrderDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Payed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Activated = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    email = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    menuId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuPersons", x => x.MenuPersonId);
                    table.ForeignKey(
                        name: "FK_MenuPersons_Menues_menuId",
                        column: x => x.menuId,
                        principalTable: "Menues",
                        principalColumn: "MenuId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MenuPersons_Persons_email",
                        column: x => x.email,
                        principalTable: "Persons",
                        principalColumn: "Email");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_MenuPersons_email",
                table: "MenuPersons",
                column: "email");

            migrationBuilder.CreateIndex(
                name: "IX_MenuPersons_menuId",
                table: "MenuPersons",
                column: "menuId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MenuPersons");

            migrationBuilder.DropTable(
                name: "Menues");

            migrationBuilder.DropTable(
                name: "Persons");
        }
    }
}
