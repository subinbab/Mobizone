using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.Migrations
{
    public partial class _11 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_productCart_userRegistrations_usersId",
                table: "productCart");

            migrationBuilder.DropIndex(
                name: "IX_productCart_usersId",
                table: "productCart");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_productCart_usersId",
                table: "productCart",
                column: "usersId");

            migrationBuilder.AddForeignKey(
                name: "FK_productCart_userRegistrations_usersId",
                table: "productCart",
                column: "usersId",
                principalTable: "userRegistrations",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
