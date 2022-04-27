using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.Migrations
{
    public partial class twentythirteenth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "storage",
                table: "Specificatiion");

            migrationBuilder.CreateTable(
                name: "Storage",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    storage = table.Column<int>(type: "int", nullable: false),
                    Specificatiionid = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Storage", x => x.id);
                    table.ForeignKey(
                        name: "FK_Storage_Specificatiion_Specificatiionid",
                        column: x => x.Specificatiionid,
                        principalTable: "Specificatiion",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "createdOn", "modifiedOn" },
                values: new object[] { new DateTime(2022, 4, 27, 0, 41, 14, 57, DateTimeKind.Local).AddTicks(8403), new DateTime(2022, 4, 27, 0, 41, 14, 57, DateTimeKind.Local).AddTicks(8773) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "createdOn", "modifiedOn" },
                values: new object[] { new DateTime(2022, 4, 27, 0, 41, 14, 57, DateTimeKind.Local).AddTicks(9059), new DateTime(2022, 4, 27, 0, 41, 14, 57, DateTimeKind.Local).AddTicks(9062) });

            migrationBuilder.CreateIndex(
                name: "IX_Storage_Specificatiionid",
                table: "Storage",
                column: "Specificatiionid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Storage");

            migrationBuilder.AddColumn<int>(
                name: "storage",
                table: "Specificatiion",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "createdOn", "modifiedOn" },
                values: new object[] { new DateTime(2022, 4, 26, 23, 22, 47, 29, DateTimeKind.Local).AddTicks(212), new DateTime(2022, 4, 26, 23, 22, 47, 29, DateTimeKind.Local).AddTicks(596) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "createdOn", "modifiedOn" },
                values: new object[] { new DateTime(2022, 4, 26, 23, 22, 47, 29, DateTimeKind.Local).AddTicks(888), new DateTime(2022, 4, 26, 23, 22, 47, 29, DateTimeKind.Local).AddTicks(890) });
        }
    }
}
