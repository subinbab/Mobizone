using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.Migrations
{
    public partial class twentyfourth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "storage",
                table: "Storage",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "ram",
                table: "Ram",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "createdOn", "modifiedOn" },
                values: new object[] { new DateTime(2022, 4, 27, 0, 50, 51, 451, DateTimeKind.Local).AddTicks(1005), new DateTime(2022, 4, 27, 0, 50, 51, 451, DateTimeKind.Local).AddTicks(1551) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "createdOn", "modifiedOn" },
                values: new object[] { new DateTime(2022, 4, 27, 0, 50, 51, 451, DateTimeKind.Local).AddTicks(2003), new DateTime(2022, 4, 27, 0, 50, 51, 451, DateTimeKind.Local).AddTicks(2006) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "storage",
                table: "Storage",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ram",
                table: "Ram",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

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
        }
    }
}
