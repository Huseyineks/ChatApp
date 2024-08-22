using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatApp.DataAccesLayer.Migrations
{
    /// <inheritdoc />
    public partial class newcolumn2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "message",
                table: "Messages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "message",
                table: "Messages");
        }
    }
}
