using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.Migrations
{
    public partial class twentyseondt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "ProductModel",
                newName: "status");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "status",
                table: "ProductModel",
                newName: "IsActive");

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
        }
    }
}
