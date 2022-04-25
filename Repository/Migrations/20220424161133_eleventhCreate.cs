using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.Migrations
{
    public partial class eleventhCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "checkOut",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userId = table.Column<int>(type: "int", nullable: false),
                    productId = table.Column<int>(type: "int", nullable: false),
                    quatity = table.Column<int>(type: "int", nullable: false),
                    paymentModeId = table.Column<int>(type: "int", nullable: false),
                    addressId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_checkOut", x => x.id);
                });

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "checkOut");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "createdOn", "modifiedOn" },
                values: new object[] { new DateTime(2022, 4, 24, 21, 36, 57, 178, DateTimeKind.Local).AddTicks(8826), new DateTime(2022, 4, 24, 21, 36, 57, 178, DateTimeKind.Local).AddTicks(9177) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "createdOn", "modifiedOn" },
                values: new object[] { new DateTime(2022, 4, 24, 21, 36, 57, 178, DateTimeKind.Local).AddTicks(9462), new DateTime(2022, 4, 24, 21, 36, 57, 178, DateTimeKind.Local).AddTicks(9465) });
        }
    }
}
