using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.Migrations
{
    public partial class twenteethCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "createdOn", "modifiedOn" },
                values: new object[] { new DateTime(2022, 4, 26, 1, 20, 42, 203, DateTimeKind.Local).AddTicks(6959), new DateTime(2022, 4, 26, 1, 20, 42, 203, DateTimeKind.Local).AddTicks(7317) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "createdOn", "modifiedOn" },
                values: new object[] { new DateTime(2022, 4, 26, 1, 20, 42, 203, DateTimeKind.Local).AddTicks(7601), new DateTime(2022, 4, 26, 1, 20, 42, 203, DateTimeKind.Local).AddTicks(7603) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "createdOn", "modifiedOn" },
                values: new object[] { new DateTime(2022, 4, 25, 2, 55, 41, 768, DateTimeKind.Local).AddTicks(7734), new DateTime(2022, 4, 25, 2, 55, 41, 768, DateTimeKind.Local).AddTicks(8279) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "createdOn", "modifiedOn" },
                values: new object[] { new DateTime(2022, 4, 25, 2, 55, 41, 768, DateTimeKind.Local).AddTicks(8695), new DateTime(2022, 4, 25, 2, 55, 41, 768, DateTimeKind.Local).AddTicks(8698) });
        }
    }
}
