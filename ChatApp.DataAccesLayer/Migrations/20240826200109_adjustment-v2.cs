using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatApp.DataAccesLayer.Migrations
{
    /// <inheritdoc />
    public partial class adjustmentv2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "userId",
                table: "OnlineUsers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "userId",
                table: "OnlineUsers",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
