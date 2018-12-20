using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.Migrations
{
    public partial class A4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_dbo_dbo_user_id",
                schema: "note",
                table: "dbo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_dbo",
                schema: "user",
                table: "dbo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_dbo",
                schema: "note",
                table: "dbo");

            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.RenameTable(
                name: "dbo",
                schema: "user",
                newName: "user",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "dbo",
                schema: "note",
                newName: "note",
                newSchema: "dbo");

            migrationBuilder.RenameIndex(
                name: "IX_dbo_user_id",
                schema: "dbo",
                table: "note",
                newName: "IX_note_user_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_user",
                schema: "dbo",
                table: "user",
                column: "user_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_note",
                schema: "dbo",
                table: "note",
                column: "note_id");

            migrationBuilder.AddForeignKey(
                name: "FK_note_user_user_id",
                schema: "dbo",
                table: "note",
                column: "user_id",
                principalSchema: "dbo",
                principalTable: "user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_note_user_user_id",
                schema: "dbo",
                table: "note");

            migrationBuilder.DropPrimaryKey(
                name: "PK_user",
                schema: "dbo",
                table: "user");

            migrationBuilder.DropPrimaryKey(
                name: "PK_note",
                schema: "dbo",
                table: "note");

            migrationBuilder.EnsureSchema(
                name: "note");

            migrationBuilder.EnsureSchema(
                name: "user");

            migrationBuilder.RenameTable(
                name: "user",
                schema: "dbo",
                newName: "dbo",
                newSchema: "user");

            migrationBuilder.RenameTable(
                name: "note",
                schema: "dbo",
                newName: "dbo",
                newSchema: "note");

            migrationBuilder.RenameIndex(
                name: "IX_note_user_id",
                schema: "note",
                table: "dbo",
                newName: "IX_dbo_user_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_dbo",
                schema: "user",
                table: "dbo",
                column: "user_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_dbo",
                schema: "note",
                table: "dbo",
                column: "note_id");

            migrationBuilder.AddForeignKey(
                name: "FK_dbo_dbo_user_id",
                schema: "note",
                table: "dbo",
                column: "user_id",
                principalSchema: "user",
                principalTable: "dbo",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
