using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatApp.DataAccesLayer.Migrations
{
    /// <inheritdoc />
    public partial class changingtypeofvariable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "reply_to",
                table: "Messages");

            migrationBuilder.AddColumn<string>(
                name: "replyingToMessage",
                table: "Messages",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "replyingToMessage",
                table: "Messages");

            migrationBuilder.AddColumn<int>(
                name: "reply_to",
                table: "Messages",
                type: "int",
                nullable: true);
        }
    }
}
