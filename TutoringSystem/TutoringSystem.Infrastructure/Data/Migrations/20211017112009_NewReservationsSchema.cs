using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TutoringSystem.Infrastructure.Data.Migrations
{
    public partial class NewReservationsSchema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OneTimeReservations",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OneTimeReservations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OneTimeReservations_Reservations_Id",
                        column: x => x.Id,
                        principalTable: "Reservations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RepeatedReservations",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastAddedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RepeatedReservations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RecurringReservations",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Frequency = table.Column<int>(type: "int", nullable: false),
                    RepeatedReservationId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecurringReservations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecurringReservations_RepeatedReservations_RepeatedReservationId",
                        column: x => x.RepeatedReservationId,
                        principalTable: "RepeatedReservations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RecurringReservations_Reservations_Id",
                        column: x => x.Id,
                        principalTable: "Reservations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RecurringReservations_RepeatedReservationId",
                table: "RecurringReservations",
                column: "RepeatedReservationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OneTimeReservations");

            migrationBuilder.DropTable(
                name: "RecurringReservations");

            migrationBuilder.DropTable(
                name: "RepeatedReservations");
        }
    }
}
