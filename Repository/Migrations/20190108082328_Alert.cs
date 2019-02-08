using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.Migrations
{
    public partial class Alert : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");
            
            migrationBuilder.CreateTable(
                name: "alert",
                schema: "dbo",
                columns: table => new
                {
                    alert_id = table.Column<long>(type: "BIGINT", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    text = table.Column<string>(type: "TEXT", nullable: false),
                    arguments = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    date_created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    from_date = table.Column<DateTime>(nullable: false),
                    to_date = table.Column<DateTime>(nullable: false),
                    date_sent = table.Column<DateTime>(type: "datetime2", nullable: false),
                    user_id = table.Column<long>(nullable: false),
                    created_on = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_on = table.Column<DateTime>(type: "datetime2", nullable: true),
                    created_by = table.Column<string>(type: "nvarchar(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_alert", x => x.alert_id);
                    table.ForeignKey(
                        name: "FK_alert_user_user_id",
                        column: x => x.user_id,
                        principalSchema: "dbo",
                        principalTable: "user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

          
            migrationBuilder.CreateIndex(
                name: "IX_alert_user_id",
                schema: "dbo",
                table: "alert",
                column: "user_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "alert",
                schema: "dbo");
            
        }
    }
}
