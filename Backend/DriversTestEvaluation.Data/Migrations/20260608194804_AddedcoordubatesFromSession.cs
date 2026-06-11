using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DriversTestEvaluation.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedcoordubatesFromSession : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Coordinates",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SessionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    x = table.Column<double>(type: "float", nullable: false),
                    y = table.Column<double>(type: "float", nullable: false),
                    DrivingSessionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coordinates", x => x.id);
                    table.ForeignKey(
                        name: "FK_Coordinates_DrivingSession_DrivingSessionId",
                        column: x => x.DrivingSessionId,
                        principalTable: "DrivingSession",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Coordinates_DrivingSessionId",
                table: "Coordinates",
                column: "DrivingSessionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Coordinates");
        }
    }
}
