using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.Migrations
{
    public partial class _12 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Address_userRegistrations_UserRegistrationUserId",
                table: "Address");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Address_addressId",
                table: "Orders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Address",
                table: "Address");

            migrationBuilder.RenameTable(
                name: "Address",
                newName: "address");

            migrationBuilder.RenameIndex(
                name: "IX_Address_UserRegistrationUserId",
                table: "address",
                newName: "IX_address_UserRegistrationUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_address",
                table: "address",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_address_userRegistrations_UserRegistrationUserId",
                table: "address",
                column: "UserRegistrationUserId",
                principalTable: "userRegistrations",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_address_addressId",
                table: "Orders",
                column: "addressId",
                principalTable: "address",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_address_userRegistrations_UserRegistrationUserId",
                table: "address");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_address_addressId",
                table: "Orders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_address",
                table: "address");

            migrationBuilder.RenameTable(
                name: "address",
                newName: "Address");

            migrationBuilder.RenameIndex(
                name: "IX_address_UserRegistrationUserId",
                table: "Address",
                newName: "IX_Address_UserRegistrationUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Address",
                table: "Address",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Address_userRegistrations_UserRegistrationUserId",
                table: "Address",
                column: "UserRegistrationUserId",
                principalTable: "userRegistrations",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Address_addressId",
                table: "Orders",
                column: "addressId",
                principalTable: "Address",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
