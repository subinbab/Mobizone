using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.Migrations
{
    public partial class _3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_checkOut_Address_addressId",
                table: "checkOut");

            migrationBuilder.DropForeignKey(
                name: "FK_checkOut_order_orderId",
                table: "checkOut");

            migrationBuilder.DropForeignKey(
                name: "FK_checkOut_ProductModel_productId",
                table: "checkOut");

            migrationBuilder.DropForeignKey(
                name: "FK_checkOut_userRegistrations_userId",
                table: "checkOut");

            migrationBuilder.DropPrimaryKey(
                name: "PK_checkOut",
                table: "checkOut");

            migrationBuilder.RenameTable(
                name: "checkOut",
                newName: "Orders");

            migrationBuilder.RenameIndex(
                name: "IX_checkOut_userId",
                table: "Orders",
                newName: "IX_Orders_userId");

            migrationBuilder.RenameIndex(
                name: "IX_checkOut_productId",
                table: "Orders",
                newName: "IX_Orders_productId");

            migrationBuilder.RenameIndex(
                name: "IX_checkOut_orderId",
                table: "Orders",
                newName: "IX_Orders_orderId");

            migrationBuilder.RenameIndex(
                name: "IX_checkOut_addressId",
                table: "Orders",
                newName: "IX_Orders_addressId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Orders",
                table: "Orders",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Address_addressId",
                table: "Orders",
                column: "addressId",
                principalTable: "Address",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_order_orderId",
                table: "Orders",
                column: "orderId",
                principalTable: "order",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_ProductModel_productId",
                table: "Orders",
                column: "productId",
                principalTable: "ProductModel",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_userRegistrations_userId",
                table: "Orders",
                column: "userId",
                principalTable: "userRegistrations",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Address_addressId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_order_orderId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_ProductModel_productId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_userRegistrations_userId",
                table: "Orders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Orders",
                table: "Orders");

            migrationBuilder.RenameTable(
                name: "Orders",
                newName: "checkOut");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_userId",
                table: "checkOut",
                newName: "IX_checkOut_userId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_productId",
                table: "checkOut",
                newName: "IX_checkOut_productId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_orderId",
                table: "checkOut",
                newName: "IX_checkOut_orderId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_addressId",
                table: "checkOut",
                newName: "IX_checkOut_addressId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_checkOut",
                table: "checkOut",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_checkOut_Address_addressId",
                table: "checkOut",
                column: "addressId",
                principalTable: "Address",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_checkOut_order_orderId",
                table: "checkOut",
                column: "orderId",
                principalTable: "order",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_checkOut_ProductModel_productId",
                table: "checkOut",
                column: "productId",
                principalTable: "ProductModel",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_checkOut_userRegistrations_userId",
                table: "checkOut",
                column: "userId",
                principalTable: "userRegistrations",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
