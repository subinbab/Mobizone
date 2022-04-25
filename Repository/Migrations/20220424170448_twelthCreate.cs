using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.Migrations
{
    public partial class twelthCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "orderId",
                table: "checkOut",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "status",
                table: "checkOut",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "createdOn", "modifiedOn" },
                values: new object[] { new DateTime(2022, 4, 24, 22, 34, 48, 312, DateTimeKind.Local).AddTicks(8951), new DateTime(2022, 4, 24, 22, 34, 48, 312, DateTimeKind.Local).AddTicks(9353) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "createdOn", "modifiedOn" },
                values: new object[] { new DateTime(2022, 4, 24, 22, 34, 48, 312, DateTimeKind.Local).AddTicks(9642), new DateTime(2022, 4, 24, 22, 34, 48, 312, DateTimeKind.Local).AddTicks(9644) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "orderId",
                table: "checkOut");

            migrationBuilder.DropColumn(
                name: "status",
                table: "checkOut");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "createdOn", "modifiedOn" },
                values: new object[] { new DateTime(2022, 4, 24, 21, 41, 33, 313, DateTimeKind.Local).AddTicks(226), new DateTime(2022, 4, 24, 21, 41, 33, 313, DateTimeKind.Local).AddTicks(598) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "createdOn", "modifiedOn" },
                values: new object[] { new DateTime(2022, 4, 24, 21, 41, 33, 313, DateTimeKind.Local).AddTicks(964), new DateTime(2022, 4, 24, 21, 41, 33, 313, DateTimeKind.Local).AddTicks(966) });
        }
    }
}
