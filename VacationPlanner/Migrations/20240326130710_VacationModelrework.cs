using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VacationPlanner.Migrations
{
    public partial class VacationModelrework : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Superior",
                table: "Vacations",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Superior",
                table: "Vacations");
        }
    }
}
