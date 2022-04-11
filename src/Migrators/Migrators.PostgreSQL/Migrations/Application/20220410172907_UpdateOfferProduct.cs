using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrators.PostgreSQL.Migrations.Application
{
    public partial class UpdateOfferProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "GrossValue",
                schema: "Catalog",
                table: "OfferProducts",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "NetValue",
                schema: "Catalog",
                table: "OfferProducts",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GrossValue",
                schema: "Catalog",
                table: "OfferProducts");

            migrationBuilder.DropColumn(
                name: "NetValue",
                schema: "Catalog",
                table: "OfferProducts");
        }
    }
}
