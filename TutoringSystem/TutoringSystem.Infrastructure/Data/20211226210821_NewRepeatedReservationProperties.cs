using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TutoringSystem.Infrastructure.Data.Migrations
{
    public partial class NewRepeatedReservationProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Duration",
                table: "RepeatedReservations",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartTime",
                table: "RepeatedReservations",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "StudentId",
                table: "RepeatedReservations",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "TutorId",
                table: "RepeatedReservations",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Duration",
                table: "RepeatedReservations");

            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "RepeatedReservations");

            migrationBuilder.DropColumn(
                name: "StudentId",
                table: "RepeatedReservations");

            migrationBuilder.DropColumn(
                name: "TutorId",
                table: "RepeatedReservations");
        }
    }
}
