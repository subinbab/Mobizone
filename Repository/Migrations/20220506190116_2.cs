using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.Migrations
{
    public partial class _2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_checkOut_ProductModel_MyPropertyid",
                table: "checkOut");

            migrationBuilder.DropForeignKey(
                name: "FK_productSubPart_Ram_ramId",
                table: "productSubPart");

            migrationBuilder.DropForeignKey(
                name: "FK_productSubPart_Storage_storageId",
                table: "productSubPart");

            migrationBuilder.DropIndex(
                name: "IX_productSubPart_ramId",
                table: "productSubPart");

            migrationBuilder.DropIndex(
                name: "IX_productSubPart_storageId",
                table: "productSubPart");

            migrationBuilder.DropIndex(
                name: "IX_checkOut_MyPropertyid",
                table: "checkOut");

            migrationBuilder.DropColumn(
                name: "MyPropertyid",
                table: "checkOut");

            migrationBuilder.RenameColumn(
                name: "quatity",
                table: "checkOut",
                newName: "quantity");

            migrationBuilder.CreateIndex(
                name: "IX_checkOut_productId",
                table: "checkOut",
                column: "productId");

            migrationBuilder.AddForeignKey(
                name: "FK_checkOut_ProductModel_productId",
                table: "checkOut",
                column: "productId",
                principalTable: "ProductModel",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_checkOut_ProductModel_productId",
                table: "checkOut");

            migrationBuilder.DropIndex(
                name: "IX_checkOut_productId",
                table: "checkOut");

            migrationBuilder.RenameColumn(
                name: "quantity",
                table: "checkOut",
                newName: "quatity");

            migrationBuilder.AddColumn<int>(
                name: "MyPropertyid",
                table: "checkOut",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_productSubPart_ramId",
                table: "productSubPart",
                column: "ramId");

            migrationBuilder.CreateIndex(
                name: "IX_productSubPart_storageId",
                table: "productSubPart",
                column: "storageId");

            migrationBuilder.CreateIndex(
                name: "IX_checkOut_MyPropertyid",
                table: "checkOut",
                column: "MyPropertyid");

            migrationBuilder.AddForeignKey(
                name: "FK_checkOut_ProductModel_MyPropertyid",
                table: "checkOut",
                column: "MyPropertyid",
                principalTable: "ProductModel",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_productSubPart_Ram_ramId",
                table: "productSubPart",
                column: "ramId",
                principalTable: "Ram",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_productSubPart_Storage_storageId",
                table: "productSubPart",
                column: "storageId",
                principalTable: "Storage",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
