using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.Migrations
{
    public partial class tentyone : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IsActive",
                table: "ProductModel",
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "ProductModel");

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
    }
}
