using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DriversTestEvaluation.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedcoordinatesFromSession : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Coordinates_DrivingSession_DrivingSessionId",
                table: "Coordinates");

            migrationBuilder.DropIndex(
                name: "IX_Coordinates_DrivingSessionId",
                table: "Coordinates");

            migrationBuilder.DropColumn(
                name: "DrivingSessionId",
                table: "Coordinates");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Coordinates",
                newName: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Coordinates_SessionId",
                table: "Coordinates",
                column: "SessionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Coordinates_DrivingSession_SessionId",
                table: "Coordinates",
                column: "SessionId",
                principalTable: "DrivingSession",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Coordinates_DrivingSession_SessionId",
                table: "Coordinates");

            migrationBuilder.DropIndex(
                name: "IX_Coordinates_SessionId",
                table: "Coordinates");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Coordinates",
                newName: "id");

            migrationBuilder.AddColumn<Guid>(
                name: "DrivingSessionId",
                table: "Coordinates",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Coordinates_DrivingSessionId",
                table: "Coordinates",
                column: "DrivingSessionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Coordinates_DrivingSession_DrivingSessionId",
                table: "Coordinates",
                column: "DrivingSessionId",
                principalTable: "DrivingSession",
                principalColumn: "Id");
        }
    }
}
