using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatApp.DataAccesLayer.Migrations
{
    /// <inheritdoc />
    public partial class dbsetusergroups : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppUserGroup_AspNetUsers_AppUserId",
                table: "AppUserGroup");

            migrationBuilder.DropForeignKey(
                name: "FK_AppUserGroup_Groups_GroupId",
                table: "AppUserGroup");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppUserGroup",
                table: "AppUserGroup");

            migrationBuilder.RenameTable(
                name: "AppUserGroup",
                newName: "UserGroups");

            migrationBuilder.RenameIndex(
                name: "IX_AppUserGroup_GroupId",
                table: "UserGroups",
                newName: "IX_UserGroups_GroupId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserGroups",
                table: "UserGroups",
                columns: new[] { "AppUserId", "GroupId" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserGroups_AspNetUsers_AppUserId",
                table: "UserGroups",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserGroups_Groups_GroupId",
                table: "UserGroups",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserGroups_AspNetUsers_AppUserId",
                table: "UserGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_UserGroups_Groups_GroupId",
                table: "UserGroups");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserGroups",
                table: "UserGroups");

            migrationBuilder.RenameTable(
                name: "UserGroups",
                newName: "AppUserGroup");

            migrationBuilder.RenameIndex(
                name: "IX_UserGroups_GroupId",
                table: "AppUserGroup",
                newName: "IX_AppUserGroup_GroupId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppUserGroup",
                table: "AppUserGroup",
                columns: new[] { "AppUserId", "GroupId" });

            migrationBuilder.AddForeignKey(
                name: "FK_AppUserGroup_AspNetUsers_AppUserId",
                table: "AppUserGroup",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppUserGroup_Groups_GroupId",
                table: "AppUserGroup",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
