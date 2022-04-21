using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.Migrations
{
    public partial class seventhCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Privacy",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    content = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Privacy", x => x.id);
                });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "createdOn", "modifiedOn" },
                values: new object[] { new DateTime(2022, 4, 19, 14, 55, 31, 791, DateTimeKind.Local).AddTicks(1324), new DateTime(2022, 4, 19, 14, 55, 31, 791, DateTimeKind.Local).AddTicks(2106) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "createdOn", "modifiedOn" },
                values: new object[] { new DateTime(2022, 4, 19, 14, 55, 31, 791, DateTimeKind.Local).AddTicks(2775), new DateTime(2022, 4, 19, 14, 55, 31, 791, DateTimeKind.Local).AddTicks(2780) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Privacy");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "createdOn", "modifiedOn" },
                values: new object[] { new DateTime(2022, 4, 19, 13, 56, 36, 249, DateTimeKind.Local).AddTicks(3956), new DateTime(2022, 4, 19, 13, 56, 36, 249, DateTimeKind.Local).AddTicks(5178) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "createdOn", "modifiedOn" },
                values: new object[] { new DateTime(2022, 4, 19, 13, 56, 36, 249, DateTimeKind.Local).AddTicks(5860), new DateTime(2022, 4, 19, 13, 56, 36, 249, DateTimeKind.Local).AddTicks(5867) });
        }
    }
}
