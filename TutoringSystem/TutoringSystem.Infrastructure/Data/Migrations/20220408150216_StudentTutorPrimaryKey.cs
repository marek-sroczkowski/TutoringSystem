using Microsoft.EntityFrameworkCore.Migrations;

namespace TutoringSystem.Infrastructure.Data.Migrations
{
    public partial class StudentTutorPrimaryKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentTutors",
                table: "StudentTutors");

            migrationBuilder.AddColumn<long>(
                name: "Id",
                table: "StudentTutors",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentTutors",
                table: "StudentTutors",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_StudentTutors_StudentId",
                table: "StudentTutors",
                column: "StudentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentTutors",
                table: "StudentTutors");

            migrationBuilder.DropIndex(
                name: "IX_StudentTutors_StudentId",
                table: "StudentTutors");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "StudentTutors");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentTutors",
                table: "StudentTutors",
                columns: new[] { "StudentId", "TutorId" });
        }
    }
}
