using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.Migrations
{
    public partial class thirteenthCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "cancelRequested",
                table: "checkOut",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "price",
                table: "checkOut",
                type: "int",
                nullable: false,
                defaultValue: 0);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "cancelRequested",
                table: "checkOut");

            migrationBuilder.DropColumn(
                name: "price",
                table: "checkOut");

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
    }
}
