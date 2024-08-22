using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatApp.DataAccesLayer.Migrations
{
    /// <inheritdoc />
    public partial class makingcodemoreunderstandable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_AspNetUsers_authorId",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_authorId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "ReceiverId",
                table: "Messages");

            migrationBuilder.RenameColumn(
                name: "authorId",
                table: "Messages",
                newName: "receiverGuid");

            migrationBuilder.AddColumn<Guid>(
                name: "userGuid",
                table: "OnlineUsers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "authorGuid",
                table: "Messages",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Messages_authorGuid",
                table: "Messages",
                column: "authorGuid");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_AspNetUsers_authorGuid",
                table: "Messages",
                column: "authorGuid",
                principalTable: "AspNetUsers",
                principalColumn: "RowGuid",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_AspNetUsers_authorGuid",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_authorGuid",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "userGuid",
                table: "OnlineUsers");

            migrationBuilder.DropColumn(
                name: "authorGuid",
                table: "Messages");

            migrationBuilder.RenameColumn(
                name: "receiverGuid",
                table: "Messages",
                newName: "authorId");

            migrationBuilder.AddColumn<string>(
                name: "ReceiverId",
                table: "Messages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_authorId",
                table: "Messages",
                column: "authorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_AspNetUsers_authorId",
                table: "Messages",
                column: "authorId",
                principalTable: "AspNetUsers",
                principalColumn: "RowGuid",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
