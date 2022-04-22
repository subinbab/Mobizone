using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.Migrations
{
    public partial class ninenthCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "additionalInfo",
                table: "Address",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "createdOn", "modifiedOn" },
                values: new object[] { new DateTime(2022, 4, 22, 4, 23, 33, 580, DateTimeKind.Local).AddTicks(656), new DateTime(2022, 4, 22, 4, 23, 33, 580, DateTimeKind.Local).AddTicks(1025) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "createdOn", "modifiedOn" },
                values: new object[] { new DateTime(2022, 4, 22, 4, 23, 33, 580, DateTimeKind.Local).AddTicks(1312), new DateTime(2022, 4, 22, 4, 23, 33, 580, DateTimeKind.Local).AddTicks(1315) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "additionalInfo",
                table: "Address");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "createdOn", "modifiedOn" },
                values: new object[] { new DateTime(2022, 4, 21, 0, 9, 31, 331, DateTimeKind.Local).AddTicks(2167), new DateTime(2022, 4, 21, 0, 9, 31, 331, DateTimeKind.Local).AddTicks(2665) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "createdOn", "modifiedOn" },
                values: new object[] { new DateTime(2022, 4, 21, 0, 9, 31, 331, DateTimeKind.Local).AddTicks(3063), new DateTime(2022, 4, 21, 0, 9, 31, 331, DateTimeKind.Local).AddTicks(3066) });
        }
    }
}
