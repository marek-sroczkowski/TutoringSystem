using Microsoft.EntityFrameworkCore.Migrations;

namespace TutoringSystem.Infrastructure.Data.Migrations
{
    public partial class AdditionalOrderStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SubjectCategory",
                table: "Subjects",
                newName: "Category");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "AdditionalOrders",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "AdditionalOrders");

            migrationBuilder.RenameColumn(
                name: "Category",
                table: "Subjects",
                newName: "SubjectCategory");
        }
    }
}
