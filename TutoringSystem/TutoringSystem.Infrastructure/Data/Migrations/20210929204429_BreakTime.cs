using Microsoft.EntityFrameworkCore.Migrations;

namespace TutoringSystem.Infrastructure.Data.Migrations
{
    public partial class BreakTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Duration",
                table: "Reservations",
                type: "int",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AddColumn<int>(
                name: "BreakTime",
                table: "Availabilities",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BreakTime",
                table: "Availabilities");

            migrationBuilder.AlterColumn<double>(
                name: "Duration",
                table: "Reservations",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
