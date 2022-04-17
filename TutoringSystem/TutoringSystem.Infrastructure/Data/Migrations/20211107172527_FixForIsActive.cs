using Microsoft.EntityFrameworkCore.Migrations;

namespace TutoringSystem.Infrastructure.Data.Migrations
{
    public partial class FixForIsActive : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsActiv",
                table: "Users",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "IsActiv",
                table: "Subjects",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "IsActiv",
                table: "PhoneNumbers",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "IsActiv",
                table: "AdditionalOrders",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "IsActiv",
                table: "ActivationTokens",
                newName: "IsActive");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "Users",
                newName: "IsActiv");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "Subjects",
                newName: "IsActiv");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "PhoneNumbers",
                newName: "IsActiv");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "AdditionalOrders",
                newName: "IsActiv");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "ActivationTokens",
                newName: "IsActiv");
        }
    }
}
