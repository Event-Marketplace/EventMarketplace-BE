using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventMarketplace.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RejectionReasonInEventEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RejectionReason",
                table: "Events",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RejectionReason",
                table: "Events");
        }
    }
}
