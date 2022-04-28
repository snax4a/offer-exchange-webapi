using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrators.PostgreSQL.Migrations.Application
{
    public partial class RemoveDeliveryCostFromOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeliveryCostDescription",
                schema: "OfferExchange",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "DeliveryCostGrossPrice",
                schema: "OfferExchange",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "DeliveryCostType",
                schema: "OfferExchange",
                table: "Orders");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DeliveryCostDescription",
                schema: "OfferExchange",
                table: "Orders",
                type: "character varying(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DeliveryCostGrossPrice",
                schema: "OfferExchange",
                table: "Orders",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<int>(
                name: "DeliveryCostType",
                schema: "OfferExchange",
                table: "Orders",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
