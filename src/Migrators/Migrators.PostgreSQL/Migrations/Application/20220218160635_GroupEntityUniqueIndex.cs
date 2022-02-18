using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrators.PostgreSQL.Migrations.Application
{
    public partial class GroupEntityUniqueIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Groups_Name_CreatedBy",
                schema: "Catalog",
                table: "Groups",
                columns: new[] { "Name", "CreatedBy" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Groups_Name_CreatedBy",
                schema: "Catalog",
                table: "Groups");
        }
    }
}
