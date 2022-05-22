using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.Migrations
{
    public partial class _13 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DbCartid",
                table: "CartDetails",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "dbCart",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    sessionId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dbCart", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CartDetails_DbCartid",
                table: "CartDetails",
                column: "DbCartid");

            migrationBuilder.AddForeignKey(
                name: "FK_CartDetails_dbCart_DbCartid",
                table: "CartDetails",
                column: "DbCartid",
                principalTable: "dbCart",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartDetails_dbCart_DbCartid",
                table: "CartDetails");

            migrationBuilder.DropTable(
                name: "dbCart");

            migrationBuilder.DropIndex(
                name: "IX_CartDetails_DbCartid",
                table: "CartDetails");

            migrationBuilder.DropColumn(
                name: "DbCartid",
                table: "CartDetails");
        }
    }
}
