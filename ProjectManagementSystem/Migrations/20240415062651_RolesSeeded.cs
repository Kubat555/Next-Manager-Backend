using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectManagementSystem.Migrations
{
    public partial class RolesSeeded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "4ee86364-f330-44c3-9a6e-bb706a7e79f0", "3", "Employee", "Employee" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "702cb572-f658-48fa-bbfa-9fdf89aa5369", "2", "Manager", "Manager" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "c966f2a6-30a9-461f-956f-6dc3c97b7e2c", "1", "Admin", "Admin" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4ee86364-f330-44c3-9a6e-bb706a7e79f0");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "702cb572-f658-48fa-bbfa-9fdf89aa5369");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c966f2a6-30a9-461f-956f-6dc3c97b7e2c");
        }
    }
}
