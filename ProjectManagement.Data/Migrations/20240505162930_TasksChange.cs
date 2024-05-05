using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectManagement.Data.Migrations
{
    public partial class TasksChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_ProjectEmployees_AuthorId",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_ProjectEmployees_ExecutorId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_AuthorId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "Tasks");

            migrationBuilder.AlterColumn<string>(
                name: "ExecutorId",
                table: "Tasks",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_AspNetUsers_ExecutorId",
                table: "Tasks",
                column: "ExecutorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_AspNetUsers_ExecutorId",
                table: "Tasks");

            migrationBuilder.AlterColumn<int>(
                name: "ExecutorId",
                table: "Tasks",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AuthorId",
                table: "Tasks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_AuthorId",
                table: "Tasks",
                column: "AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_ProjectEmployees_AuthorId",
                table: "Tasks",
                column: "AuthorId",
                principalTable: "ProjectEmployees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_ProjectEmployees_ExecutorId",
                table: "Tasks",
                column: "ExecutorId",
                principalTable: "ProjectEmployees",
                principalColumn: "Id");
        }
    }
}
