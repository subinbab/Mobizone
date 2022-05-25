using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.Migrations
{
    public partial class _5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MyCartid",
                table: "CartDetails",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "myCart",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    sessionId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    usersId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_myCart", x => x.id);
                    table.ForeignKey(
                        name: "FK_myCart_userRegistrations_usersId",
                        column: x => x.usersId,
                        principalTable: "userRegistrations",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CartDetails_MyCartid",
                table: "CartDetails",
                column: "MyCartid");

            migrationBuilder.CreateIndex(
                name: "IX_myCart_usersId",
                table: "myCart",
                column: "usersId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartDetails_myCart_MyCartid",
                table: "CartDetails",
                column: "MyCartid",
                principalTable: "myCart",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartDetails_myCart_MyCartid",
                table: "CartDetails");

            migrationBuilder.DropTable(
                name: "myCart");

            migrationBuilder.DropIndex(
                name: "IX_CartDetails_MyCartid",
                table: "CartDetails");

            migrationBuilder.DropColumn(
                name: "MyCartid",
                table: "CartDetails");
        }
    }
}
