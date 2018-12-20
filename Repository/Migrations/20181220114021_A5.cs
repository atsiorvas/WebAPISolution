using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.Migrations
{
    public partial class A5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "alert",
                newName: "alert",
                newSchema: "dbo");

            migrationBuilder.RenameColumn(
                name: "AuditedEntity_UpdatedOn",
                schema: "dbo",
                table: "alert",
                newName: "updated_on");

            migrationBuilder.RenameColumn(
                name: "AuditedEntity_CreatedOn",
                schema: "dbo",
                table: "alert",
                newName: "created_on");

            migrationBuilder.RenameColumn(
                name: "AuditedEntity_CreatedBy",
                schema: "dbo",
                table: "alert",
                newName: "created_by");

            migrationBuilder.AlterColumn<string>(
                name: "text",
                schema: "dbo",
                table: "alert",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "arguments",
                schema: "dbo",
                table: "alert",
                type: "nvarchar(100)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "alert_id",
                schema: "dbo",
                table: "alert",
                type: "BIGINT",
                nullable: false,
                oldClrType: typeof(long))
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn)
                .OldAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<string>(
                name: "created_by",
                schema: "dbo",
                table: "alert",
                type: "nvarchar(100)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "alert",
                schema: "dbo",
                newName: "alert");

            migrationBuilder.RenameColumn(
                name: "updated_on",
                table: "alert",
                newName: "AuditedEntity_UpdatedOn");

            migrationBuilder.RenameColumn(
                name: "created_on",
                table: "alert",
                newName: "AuditedEntity_CreatedOn");

            migrationBuilder.RenameColumn(
                name: "created_by",
                table: "alert",
                newName: "AuditedEntity_CreatedBy");

            migrationBuilder.AlterColumn<string>(
                name: "text",
                table: "alert",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "arguments",
                table: "alert",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)");

            migrationBuilder.AlterColumn<long>(
                name: "alert_id",
                table: "alert",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "BIGINT")
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn)
                .OldAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<string>(
                name: "AuditedEntity_CreatedBy",
                table: "alert",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)");
        }
    }
}
