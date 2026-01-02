using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventMarketplace.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class EventCommentEntityAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreateAt",
                table: "Users",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "CreateAt",
                table: "Roles",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "CreateAt",
                table: "RefreshTokens",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "CreateAt",
                table: "Events",
                newName: "CreatedAt");

            migrationBuilder.CreateTable(
                name: "EventComments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    WasReadByAdmin = table.Column<bool>(type: "boolean", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    EventId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventComments_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_EventComments_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventComments_EventId",
                table: "EventComments",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_EventComments_UserId",
                table: "EventComments",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventComments");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Users",
                newName: "CreateAt");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Roles",
                newName: "CreateAt");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "RefreshTokens",
                newName: "CreateAt");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Events",
                newName: "CreateAt");
        }
    }
}
