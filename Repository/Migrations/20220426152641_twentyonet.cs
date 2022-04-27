using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.Migrations
{
    public partial class twentyonet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ram",
                table: "Specificatiion");

            migrationBuilder.CreateTable(
                name: "Ram",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ram = table.Column<int>(type: "int", nullable: false),
                    Specificatiionid = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ram", x => x.id);
                    table.ForeignKey(
                        name: "FK_Ram_Specificatiion_Specificatiionid",
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
                values: new object[] { new DateTime(2022, 4, 26, 20, 56, 41, 180, DateTimeKind.Local).AddTicks(1399), new DateTime(2022, 4, 26, 20, 56, 41, 180, DateTimeKind.Local).AddTicks(1762) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "createdOn", "modifiedOn" },
                values: new object[] { new DateTime(2022, 4, 26, 20, 56, 41, 180, DateTimeKind.Local).AddTicks(2044), new DateTime(2022, 4, 26, 20, 56, 41, 180, DateTimeKind.Local).AddTicks(2046) });

            migrationBuilder.CreateIndex(
                name: "IX_Ram_Specificatiionid",
                table: "Ram",
                column: "Specificatiionid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ram");

            migrationBuilder.AddColumn<int>(
                name: "ram",
                table: "Specificatiion",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "createdOn", "modifiedOn" },
                values: new object[] { new DateTime(2022, 4, 26, 1, 24, 25, 725, DateTimeKind.Local).AddTicks(3980), new DateTime(2022, 4, 26, 1, 24, 25, 725, DateTimeKind.Local).AddTicks(4351) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "createdOn", "modifiedOn" },
                values: new object[] { new DateTime(2022, 4, 26, 1, 24, 25, 725, DateTimeKind.Local).AddTicks(4646), new DateTime(2022, 4, 26, 1, 24, 25, 725, DateTimeKind.Local).AddTicks(4648) });
        }
    }
}
