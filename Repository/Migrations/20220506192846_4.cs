using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.Migrations
{
    public partial class _4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_order_orderId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_orderId",
                table: "Orders");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Orders_orderId",
                table: "Orders",
                column: "orderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_order_orderId",
                table: "Orders",
                column: "orderId",
                principalTable: "order",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
