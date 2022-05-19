using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.Migrations
{
    public partial class _6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "cart",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_cart_UserId",
                table: "cart",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_cart_userRegistrations_UserId",
                table: "cart",
                column: "UserId",
                principalTable: "userRegistrations",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_cart_userRegistrations_UserId",
                table: "cart");

            migrationBuilder.DropIndex(
                name: "IX_cart_UserId",
                table: "cart");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "cart");
        }
    }
}
