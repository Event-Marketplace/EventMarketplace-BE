using EventMarketplace.Infrastructure.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventMarketplace.Infrastructure.Migrations;

[DbContext(typeof(EventMarketplaceDbContext))]
[Migration("20261027120000_EventRegistrationAdded")]
public partial class EventRegistrationAdded : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "EventRegistrations",
            columns: table => new
            {
                EventId = table.Column<Guid>(type: "uuid", nullable: false),
                ParticipantId = table.Column<Guid>(type: "uuid", nullable: false),
                RegisteredAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_EventRegistrations", x => new { x.EventId, x.ParticipantId });
                table.ForeignKey(
                    name: "FK_EventRegistrations_Events_EventId",
                    column: x => x.EventId,
                    principalTable: "Events",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_EventRegistrations_Users_ParticipantId",
                    column: x => x.ParticipantId,
                    principalTable: "Users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_EventRegistrations_ParticipantId",
            table: "EventRegistrations",
            column: "ParticipantId");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "EventRegistrations");
    }
}
