using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.Migrations
{
    public partial class _10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            

          
            migrationBuilder.CreateTable(
                name: "productCart",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    sessionId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    usersId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_productCart", x => x.id);
                    table.ForeignKey(
                        name: "FK_productCart_userRegistrations_usersId",
                        column: x => x.usersId,
                        principalTable: "userRegistrations",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            

           
            migrationBuilder.CreateIndex(
                name: "IX_productCart_usersId",
                table: "productCart",
                column: "usersId");

            
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "About");

            migrationBuilder.DropTable(
                name: "AdminContact");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "CartDetails");

            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.DropTable(
                name: "login");

            migrationBuilder.DropTable(
                name: "Master");

            migrationBuilder.DropTable(
                name: "order");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Privacy");

            migrationBuilder.DropTable(
                name: "products");

            migrationBuilder.DropTable(
                name: "Ram");

            migrationBuilder.DropTable(
                name: "Storage");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "cart");

            migrationBuilder.DropTable(
                name: "productCart");

            migrationBuilder.DropTable(
                name: "productSubPart");

            migrationBuilder.DropTable(
                name: "roles");

            migrationBuilder.DropTable(
                name: "Address");

            migrationBuilder.DropTable(
                name: "Color");

            migrationBuilder.DropTable(
                name: "userRegistrations");

            migrationBuilder.DropTable(
                name: "ProductModel");

            migrationBuilder.DropTable(
                name: "Specificatiion");
        }
    }
}
