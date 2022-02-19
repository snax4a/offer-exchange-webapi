using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrators.PostgreSQL.Migrations.Application
{
    public partial class UpdateGroupEntityIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Groups_Name_CreatedBy",
                schema: "Catalog",
                table: "Groups");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_Name_CreatedBy",
                schema: "Catalog",
                table: "Groups",
                columns: new[] { "Name", "CreatedBy" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Groups_Name_CreatedBy",
                schema: "Catalog",
                table: "Groups");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_Name_CreatedBy",
                schema: "Catalog",
                table: "Groups",
                columns: new[] { "Name", "CreatedBy" },
                unique: true);
        }
    }
}
