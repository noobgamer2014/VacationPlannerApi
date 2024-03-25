using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VacationPlanner.Migrations
{
    public partial class UpdateVacationModel2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vacations_Users_UserId",
                table: "Vacations");

            migrationBuilder.DropIndex(
                name: "IX_Vacations_UserId",
                table: "Vacations");

            migrationBuilder.DropColumn(
                name: "MaxVacationDays",
                table: "Users");

            migrationBuilder.AlterColumn<double>(
                name: "UserId",
                table: "Vacations",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<DateTime>(
                name: "RequestDate",
                table: "Vacations",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "UserId1",
                table: "Vacations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "VacationDays",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Vacations_UserId1",
                table: "Vacations",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Vacations_Users_UserId1",
                table: "Vacations",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vacations_Users_UserId1",
                table: "Vacations");

            migrationBuilder.DropIndex(
                name: "IX_Vacations_UserId1",
                table: "Vacations");

            migrationBuilder.DropColumn(
                name: "RequestDate",
                table: "Vacations");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Vacations");

            migrationBuilder.DropColumn(
                name: "VacationDays",
                table: "Users");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Vacations",
                type: "int",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AddColumn<double>(
                name: "MaxVacationDays",
                table: "Users",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateIndex(
                name: "IX_Vacations_UserId",
                table: "Vacations",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Vacations_Users_UserId",
                table: "Vacations",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
