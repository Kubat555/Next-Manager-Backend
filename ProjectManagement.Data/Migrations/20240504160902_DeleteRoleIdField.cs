using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectManagement.Data.Migrations
{
    public partial class DeleteRoleIdField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectEmployees_AspNetRoles_RoleId",
                table: "ProjectEmployees");

            migrationBuilder.DropIndex(
                name: "IX_ProjectEmployees_RoleId",
                table: "ProjectEmployees");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7dfe47f1-bd8f-4fde-acd8-77ed53fc5452");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a5a03984-f1e9-4bdb-af22-4af007a37dd3");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ddf55c02-04ba-4fdf-b6e4-10bc9b273788");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "ProjectEmployees");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RoleId",
                table: "ProjectEmployees",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "7dfe47f1-bd8f-4fde-acd8-77ed53fc5452", "2", "Manager", "Manager" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "a5a03984-f1e9-4bdb-af22-4af007a37dd3", "3", "Employee", "Employee" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "ddf55c02-04ba-4fdf-b6e4-10bc9b273788", "1", "Admin", "Admin" });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectEmployees_RoleId",
                table: "ProjectEmployees",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectEmployees_AspNetRoles_RoleId",
                table: "ProjectEmployees",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id");
        }
    }
}
