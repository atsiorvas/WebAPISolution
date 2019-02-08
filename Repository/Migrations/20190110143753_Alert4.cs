using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.Migrations
{
    public partial class Alert4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_order_user_customer_id",
                schema: "dbo",
                table: "order");

            migrationBuilder.RenameColumn(
                name: "customer_id",
                schema: "dbo",
                table: "order",
                newName: "user_id");

            migrationBuilder.RenameIndex(
                name: "IX_order_customer_id",
                schema: "dbo",
                table: "order",
                newName: "IX_order_user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_order_user_user_id",
                schema: "dbo",
                table: "order",
                column: "user_id",
                principalSchema: "dbo",
                principalTable: "user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_order_user_user_id",
                schema: "dbo",
                table: "order");

            migrationBuilder.RenameColumn(
                name: "user_id",
                schema: "dbo",
                table: "order",
                newName: "customer_id");

            migrationBuilder.RenameIndex(
                name: "IX_order_user_id",
                schema: "dbo",
                table: "order",
                newName: "IX_order_customer_id");

            migrationBuilder.AddForeignKey(
                name: "FK_order_user_customer_id",
                schema: "dbo",
                table: "order",
                column: "customer_id",
                principalSchema: "dbo",
                principalTable: "user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
