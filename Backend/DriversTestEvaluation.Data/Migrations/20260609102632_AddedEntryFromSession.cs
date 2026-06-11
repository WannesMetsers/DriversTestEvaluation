using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DriversTestEvaluation.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedEntryFromSession : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Entries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SessionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Position = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Speed_kmh = table.Column<double>(type: "float", nullable: false),
                    SpaceCarInFront = table.Column<double>(type: "float", nullable: false),
                    ColorTrafficLight = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InFrontOfTrafficLight = table.Column<bool>(type: "bit", nullable: false),
                    SpeedLimit = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Entries_DrivingSession_SessionId",
                        column: x => x.SessionId,
                        principalTable: "DrivingSession",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Entries_SessionId",
                table: "Entries",
                column: "SessionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Entries");
        }
    }
}
