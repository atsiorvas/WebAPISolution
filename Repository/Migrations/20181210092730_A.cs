using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.Migrations
{
    public partial class A : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notes_User_UserId",
                table: "Notes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_User",
                table: "User");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Notes",
                table: "Notes");

            migrationBuilder.RenameTable(
                name: "User",
                newName: "user");

            migrationBuilder.RenameTable(
                name: "Notes",
                newName: "note");

            migrationBuilder.RenameColumn(
                name: "ResetAnswer",
                table: "user",
                newName: "reset_answer");

            migrationBuilder.RenameColumn(
                name: "RememberMe",
                table: "user",
                newName: "remember_me");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "note",
                newName: "user_id");

            migrationBuilder.RenameIndex(
                name: "IX_Notes_UserId",
                table: "note",
                newName: "IX_note_user_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_user",
                table: "user",
                column: "user_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_note",
                table: "note",
                column: "note_id");

            migrationBuilder.AddForeignKey(
                name: "FK_note_user_user_id",
                table: "note",
                column: "user_id",
                principalTable: "user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_note_user_user_id",
                table: "note");

            migrationBuilder.DropPrimaryKey(
                name: "PK_user",
                table: "user");

            migrationBuilder.DropPrimaryKey(
                name: "PK_note",
                table: "note");

            migrationBuilder.RenameTable(
                name: "user",
                newName: "User");

            migrationBuilder.RenameTable(
                name: "note",
                newName: "Notes");

            migrationBuilder.RenameColumn(
                name: "reset_answer",
                table: "User",
                newName: "ResetAnswer");

            migrationBuilder.RenameColumn(
                name: "remember_me",
                table: "User",
                newName: "RememberMe");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "Notes",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_note_user_id",
                table: "Notes",
                newName: "IX_Notes_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_User",
                table: "User",
                column: "user_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Notes",
                table: "Notes",
                column: "note_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Notes_User_UserId",
                table: "Notes",
                column: "UserId",
                principalTable: "User",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
