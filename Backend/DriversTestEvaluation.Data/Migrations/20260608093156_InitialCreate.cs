using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DriversTestEvaluation.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DrivingSession",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Score = table.Column<int>(type: "int", nullable: false),
                    SessionActive = table.Column<bool>(type: "bit", nullable: false),
                    GameWindowName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DrivingSession", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Results",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GameName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Passed = table.Column<bool>(type: "bit", nullable: false),
                    Score = table.Column<int>(type: "int", nullable: false),
                    NumberOfEvents = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Results", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DrivingEvent",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EventType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Penalty = table.Column<int>(type: "int", nullable: false),
                    DrivingSessionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ResultsId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DrivingEvent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DrivingEvent_DrivingSession_DrivingSessionId",
                        column: x => x.DrivingSessionId,
                        principalTable: "DrivingSession",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DrivingEvent_Results_ResultsId",
                        column: x => x.ResultsId,
                        principalTable: "Results",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_DrivingEvent_DrivingSessionId",
                table: "DrivingEvent",
                column: "DrivingSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_DrivingEvent_ResultsId",
                table: "DrivingEvent",
                column: "ResultsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DrivingEvent");

            migrationBuilder.DropTable(
                name: "DrivingSession");

            migrationBuilder.DropTable(
                name: "Results");
        }
    }
}
