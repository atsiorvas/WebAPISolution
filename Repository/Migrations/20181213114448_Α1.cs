using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.Migrations
{
    public partial class Α1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "note");

            migrationBuilder.EnsureSchema(
                name: "user");

            migrationBuilder.CreateTable(
                name: "user",
                schema: "user",
                columns: table => new
                {
                    user_id = table.Column<long>(type: "BIGINT", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    first_name = table.Column<string>(maxLength: 100, nullable: true),
                    last_name = table.Column<string>(maxLength: 100, nullable: true),
                    password = table.Column<string>(maxLength: 100, nullable: false),
                    email = table.Column<string>(nullable: false),
                    remember_me = table.Column<bool>(nullable: false),
                    is_admin = table.Column<bool>(nullable: false),
                    reset_answer = table.Column<string>(nullable: false),
                    created_on = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_on = table.Column<DateTime>(type: "datetime2", nullable: true),
                    created_by = table.Column<string>(type: "nvarchar(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user", x => x.user_id);
                });

            migrationBuilder.CreateTable(
                name: "note",
                schema: "note",
                columns: table => new
                {
                    note_id = table.Column<long>(type: "BIGINT", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    text = table.Column<string>(type: "TEXT", nullable: false),
                    lang = table.Column<byte>(type: "TINYINT", nullable: false),
                    user_id = table.Column<long>(nullable: false),
                    created_on = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_on = table.Column<DateTime>(type: "datetime2", nullable: true),
                    created_by = table.Column<string>(type: "nvarchar(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_note", x => x.note_id);
                    table.ForeignKey(
                        name: "FK_note_user_user_id",
                        column: x => x.user_id,
                        principalSchema: "user",
                        principalTable: "user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_note_user_id",
                schema: "note",
                table: "note",
                column: "user_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "note",
                schema: "note");

            migrationBuilder.DropTable(
                name: "user",
                schema: "user");
        }
    }
}
