using Microsoft.EntityFrameworkCore.Migrations;

namespace TutoringSystem.Infrastructure.Data.Migrations
{
    public partial class FixInHourRate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudentTutor");

            migrationBuilder.DropColumn(
                name: "HourlRate",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "Students");

            migrationBuilder.CreateTable(
                name: "StudentTutors",
                columns: table => new
                {
                    StudentId = table.Column<long>(type: "bigint", nullable: false),
                    TutorId = table.Column<long>(type: "bigint", nullable: false),
                    HourlRate = table.Column<double>(type: "float", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentTutors", x => new { x.StudentId, x.TutorId });
                    table.ForeignKey(
                        name: "FK_StudentTutors_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentTutors_Tutors_TutorId",
                        column: x => x.TutorId,
                        principalTable: "Tutors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudentTutors_TutorId",
                table: "StudentTutors",
                column: "TutorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudentTutors");

            migrationBuilder.AddColumn<double>(
                name: "HourlRate",
                table: "Students",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "Students",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "StudentTutor",
                columns: table => new
                {
                    StudentsId = table.Column<long>(type: "bigint", nullable: false),
                    TutorsId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentTutor", x => new { x.StudentsId, x.TutorsId });
                    table.ForeignKey(
                        name: "FK_StudentTutor_Students_StudentsId",
                        column: x => x.StudentsId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentTutor_Tutors_TutorsId",
                        column: x => x.TutorsId,
                        principalTable: "Tutors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudentTutor_TutorsId",
                table: "StudentTutor",
                column: "TutorsId");
        }
    }
}
