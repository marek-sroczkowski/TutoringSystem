using Microsoft.EntityFrameworkCore.Migrations;

namespace TutoringSystem.Infrastructure.Data.Migrations
{
    public partial class IsActiveInRepeatedReservation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "RepeatedReservations",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "RepeatedReservations");
        }
    }
}
