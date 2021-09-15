using Microsoft.EntityFrameworkCore.Migrations;

namespace TutoringSystem.Infrastructure.Data.Migrations
{
    public partial class SubjectActivationProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActiv",
                table: "Subjects",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActiv",
                table: "Subjects");
        }
    }
}
