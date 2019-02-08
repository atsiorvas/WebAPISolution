using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.Migrations {
    public partial class Alert1 : Migration {
        protected override void Up(MigrationBuilder migrationBuilder) {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "alert",
                schema: "dbo",
                columns: table => new {
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
                constraints: table => {
                    table.PrimaryKey("PK_alert", x => x.alert_id);
                    table.ForeignKey(
                        name: "FK_alert_user_user_id",
                        column: x => x.user_id,
                        principalSchema: "dbo",
                        principalTable: "user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "order",
                schema: "dbo",
                columns: table => new {
                    order_id = table.Column<long>(type: "BIGINT", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    date_sent = table.Column<DateTime>(nullable: false),
                    text = table.Column<string>(type: "nvarchar(200)", nullable: false),
                    customer_id = table.Column<long>(nullable: false),
                    created_on = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_on = table.Column<DateTime>(type: "datetime2", nullable: true),
                    created_by = table.Column<string>(type: "nvarchar(100)", nullable: false)
                },
                constraints: table => {
                    table.PrimaryKey("PK_order", x => x.order_id);
                    table.ForeignKey(
                        name: "FK_order_user_customer_id",
                        column: x => x.customer_id,
                        principalSchema: "dbo",
                        principalTable: "user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "order_alert",
                columns: table => new {
                    order_alert_id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    order_id = table.Column<long>(nullable: false),
                    alert_id = table.Column<long>(nullable: false)
                },
                constraints: table => {
                    table.PrimaryKey("PK_order_alert", x => x.order_alert_id);
                    table.ForeignKey(
                        name: "FK_order_alert_alert_alert_id",
                        column: x => x.alert_id,
                        principalSchema: "dbo",
                        principalTable: "alert",
                        principalColumn: "alert_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_order_alert_order_order_id",
                        column: x => x.order_id,
                        principalSchema: "dbo",
                        principalTable: "order",
                        principalColumn: "order_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_order_alert_alert_id",
                table: "order_alert",
                column: "alert_id");

            migrationBuilder.CreateIndex(
                name: "IX_order_alert_order_id",
                table: "order_alert",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "IX_alert_user_id",
                schema: "dbo",
                table: "alert",
                column: "user_id");

           
            migrationBuilder.CreateIndex(
                name: "IX_order_customer_id",
                schema: "dbo",
                table: "order",
                column: "customer_id");
            
        }

        protected override void Down(MigrationBuilder migrationBuilder) {
            migrationBuilder.DropTable(
                name: "order_alert");

            migrationBuilder.DropTable(
                name: "alert",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "order",
                schema: "dbo");


        }
    }
}
