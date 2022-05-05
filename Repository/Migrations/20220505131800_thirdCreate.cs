using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.Migrations
{
    public partial class thirdCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_login_Roles_rolesid",
                table: "login");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Roles",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "roleId",
                table: "login");

            migrationBuilder.RenameTable(
                name: "Roles",
                newName: "roles");

            migrationBuilder.RenameColumn(
                name: "rolesid",
                table: "login",
                newName: "rolesId");

            migrationBuilder.RenameIndex(
                name: "IX_login_rolesid",
                table: "login",
                newName: "IX_login_rolesId");

            migrationBuilder.AlterColumn<int>(
                name: "rolesId",
                table: "login",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_roles",
                table: "roles",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_login_roles_rolesId",
                table: "login",
                column: "rolesId",
                principalTable: "roles",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_login_roles_rolesId",
                table: "login");

            migrationBuilder.DropPrimaryKey(
                name: "PK_roles",
                table: "roles");

            migrationBuilder.RenameTable(
                name: "roles",
                newName: "Roles");

            migrationBuilder.RenameColumn(
                name: "rolesId",
                table: "login",
                newName: "rolesid");

            migrationBuilder.RenameIndex(
                name: "IX_login_rolesId",
                table: "login",
                newName: "IX_login_rolesid");

            migrationBuilder.AlterColumn<int>(
                name: "rolesid",
                table: "login",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "roleId",
                table: "login",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Roles",
                table: "Roles",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_login_Roles_rolesid",
                table: "login",
                column: "rolesid",
                principalTable: "Roles",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
