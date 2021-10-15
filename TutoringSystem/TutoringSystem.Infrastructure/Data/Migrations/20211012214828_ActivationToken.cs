using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TutoringSystem.Infrastructure.Data.Migrations
{
    public partial class ActivationToken : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsEnable",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "ActivationTokens",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TokenContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActiv = table.Column<bool>(type: "bit", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivationTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActivationTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActivationTokens_UserId",
                table: "ActivationTokens",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActivationTokens");

            migrationBuilder.DropColumn(
                name: "IsEnable",
                table: "Users");
        }
    }
}
