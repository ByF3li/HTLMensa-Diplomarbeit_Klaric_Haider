using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MensaWebsite.Migrations
{
    /// <inheritdoc />
    public partial class person : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Persons",
                columns: table => new
                {
                    Email = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FirstName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persons", x => x.Email);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Prices",
                columns: table => new
                {
                    PriceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PriceStudent = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    PriceTeacher = table.Column<decimal>(type: "decimal(5,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prices", x => x.PriceId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Menues",
                columns: table => new
                {
                    MenuId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Starter = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MainCourse = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    PricesPriceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Menues", x => x.MenuId);
                    table.ForeignKey(
                        name: "FK_Menues_Prices_PricesPriceId",
                        column: x => x.PricesPriceId,
                        principalTable: "Prices",
                        principalColumn: "PriceId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "
                s",
                columns: table => new
                {
                    MenuPersonId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    OrderDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Payed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Activated = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    InShoppingcart = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    PersonEmail = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MenuId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuPersons", x => x.MenuPersonId);
                    table.ForeignKey(
                        name: "FK_MenuPersons_Menues_MenuId",
                        column: x => x.MenuId,
                        principalTable: "Menues",
                        principalColumn: "MenuId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MenuPersons_Persons_PersonEmail",
                        column: x => x.PersonEmail,
                        principalTable: "Persons",
                        principalColumn: "Email",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Menues_PricesPriceId",
                table: "Menues",
                column: "PricesPriceId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuPersons_MenuId",
                table: "MenuPersons",
                column: "MenuId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuPersons_PersonEmail",
                table: "MenuPersons",
                column: "PersonEmail");
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

            migrationBuilder.DropTable(
                name: "Prices");
        }
    }
}
