using Microsoft.EntityFrameworkCore.Migrations;

namespace TutoringSystem.Infrastructure.Data.Migrations
{
    public partial class RenamedResEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OneTimeReservations_Reservations_Id",
                table: "OneTimeReservations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OneTimeReservations",
                table: "OneTimeReservations");

            migrationBuilder.RenameTable(
                name: "OneTimeReservations",
                newName: "SingleReservations");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SingleReservations",
                table: "SingleReservations",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SingleReservations_Reservations_Id",
                table: "SingleReservations",
                column: "Id",
                principalTable: "Reservations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SingleReservations_Reservations_Id",
                table: "SingleReservations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SingleReservations",
                table: "SingleReservations");

            migrationBuilder.RenameTable(
                name: "SingleReservations",
                newName: "OneTimeReservations");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OneTimeReservations",
                table: "OneTimeReservations",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OneTimeReservations_Reservations_Id",
                table: "OneTimeReservations",
                column: "Id",
                principalTable: "Reservations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
