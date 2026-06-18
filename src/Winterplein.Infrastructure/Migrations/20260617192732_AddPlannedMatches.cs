using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Winterplein.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPlannedMatches : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PlannedMatches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SeasonId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    Team1Player1PlayerId = table.Column<int>(type: "int", nullable: false),
                    Team1Player1FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Team1Player1LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Team1Player1Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Team1Player2PlayerId = table.Column<int>(type: "int", nullable: false),
                    Team1Player2FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Team1Player2LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Team1Player2Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Team2Player1PlayerId = table.Column<int>(type: "int", nullable: false),
                    Team2Player1FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Team2Player1LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Team2Player1Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Team2Player2PlayerId = table.Column<int>(type: "int", nullable: false),
                    Team2Player2FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Team2Player2LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Team2Player2Gender = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlannedMatches", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlannedMatches");
        }
    }
}
