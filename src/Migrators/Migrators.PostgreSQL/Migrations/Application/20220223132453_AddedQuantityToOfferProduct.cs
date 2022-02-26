using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrators.PostgreSQL.Migrations.Application
{
    public partial class AddedQuantityToOfferProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "VatRate",
                schema: "Catalog",
                table: "OfferProducts",
                type: "numeric(3,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(3,2)");

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                schema: "Catalog",
                table: "OfferProducts",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                schema: "Catalog",
                table: "OfferProducts");

            migrationBuilder.AlterColumn<decimal>(
                name: "VatRate",
                schema: "Catalog",
                table: "OfferProducts",
                type: "numeric(3,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "numeric(3,2)",
                oldNullable: true);
        }
    }
}
