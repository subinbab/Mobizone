using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.Migrations
{
    public partial class ninthCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "createdOn", "modifiedOn" },
                values: new object[] { new DateTime(2022, 4, 21, 17, 53, 38, 543, DateTimeKind.Local).AddTicks(2306), new DateTime(2022, 4, 21, 17, 53, 38, 543, DateTimeKind.Local).AddTicks(3211) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "createdOn", "modifiedOn" },
                values: new object[] { new DateTime(2022, 4, 21, 17, 53, 38, 543, DateTimeKind.Local).AddTicks(3893), new DateTime(2022, 4, 21, 17, 53, 38, 543, DateTimeKind.Local).AddTicks(3901) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
