using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrators.PostgreSQL.Migrations.Application
{
    public partial class ChangeDecimalsToBigInts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "NetValue",
                schema: "Catalog",
                table: "Orders",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)");

            migrationBuilder.AlterColumn<long>(
                name: "GrossValue",
                schema: "Catalog",
                table: "Orders",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)");

            migrationBuilder.AlterColumn<long>(
                name: "DeliveryCostGrossPrice",
                schema: "Catalog",
                table: "Orders",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)");

            migrationBuilder.AlterColumn<long>(
                name: "NetValue",
                schema: "Catalog",
                table: "Offers",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)");

            migrationBuilder.AlterColumn<long>(
                name: "GrossValue",
                schema: "Catalog",
                table: "Offers",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)");

            migrationBuilder.AlterColumn<long>(
                name: "DeliveryCostGrossPrice",
                schema: "Catalog",
                table: "Offers",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)");

            migrationBuilder.AlterColumn<short>(
                name: "VatRate",
                schema: "Catalog",
                table: "OfferProducts",
                type: "smallint",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(3,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "NetPrice",
                schema: "Catalog",
                table: "OfferProducts",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "NetValue",
                schema: "Catalog",
                table: "Orders",
                type: "numeric(18,2)",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<decimal>(
                name: "GrossValue",
                schema: "Catalog",
                table: "Orders",
                type: "numeric(18,2)",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<decimal>(
                name: "DeliveryCostGrossPrice",
                schema: "Catalog",
                table: "Orders",
                type: "numeric(18,2)",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<decimal>(
                name: "NetValue",
                schema: "Catalog",
                table: "Offers",
                type: "numeric(18,2)",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<decimal>(
                name: "GrossValue",
                schema: "Catalog",
                table: "Offers",
                type: "numeric(18,2)",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<decimal>(
                name: "DeliveryCostGrossPrice",
                schema: "Catalog",
                table: "Offers",
                type: "numeric(18,2)",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<decimal>(
                name: "VatRate",
                schema: "Catalog",
                table: "OfferProducts",
                type: "numeric(3,2)",
                nullable: true,
                oldClrType: typeof(short),
                oldType: "smallint",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "NetPrice",
                schema: "Catalog",
                table: "OfferProducts",
                type: "numeric(18,2)",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");
        }
    }
}
