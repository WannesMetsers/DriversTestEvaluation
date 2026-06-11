using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DriversTestEvaluation.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFixed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DrivingEvent_DrivingSession_DrivingSessionId",
                table: "DrivingEvent");

            migrationBuilder.DropIndex(
                name: "IX_DrivingEvent_DrivingSessionId",
                table: "DrivingEvent");

            migrationBuilder.DropColumn(
                name: "DrivingSessionId",
                table: "DrivingEvent");

            migrationBuilder.AddColumn<Guid>(
                name: "SessionId",
                table: "DrivingEvent",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_DrivingEvent_SessionId",
                table: "DrivingEvent",
                column: "SessionId");

            migrationBuilder.AddForeignKey(
                name: "FK_DrivingEvent_DrivingSession_SessionId",
                table: "DrivingEvent",
                column: "SessionId",
                principalTable: "DrivingSession",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DrivingEvent_DrivingSession_SessionId",
                table: "DrivingEvent");

            migrationBuilder.DropIndex(
                name: "IX_DrivingEvent_SessionId",
                table: "DrivingEvent");

            migrationBuilder.DropColumn(
                name: "SessionId",
                table: "DrivingEvent");

            migrationBuilder.AddColumn<Guid>(
                name: "DrivingSessionId",
                table: "DrivingEvent",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DrivingEvent_DrivingSessionId",
                table: "DrivingEvent",
                column: "DrivingSessionId");

            migrationBuilder.AddForeignKey(
                name: "FK_DrivingEvent_DrivingSession_DrivingSessionId",
                table: "DrivingEvent",
                column: "DrivingSessionId",
                principalTable: "DrivingSession",
                principalColumn: "Id");
        }
    }
}
