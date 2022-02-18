using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrators.PostgreSQL.Migrations.Application
{
    public partial class EntitiesCleanup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DeliveryCostPrice",
                schema: "Catalog",
                table: "Offers",
                newName: "DeliveryCostGrossPrice");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DeliveryCostGrossPrice",
                schema: "Catalog",
                table: "Offers",
                newName: "DeliveryCostPrice");
        }
    }
}
