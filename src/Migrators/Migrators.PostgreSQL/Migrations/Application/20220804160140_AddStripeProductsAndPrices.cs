using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrators.PostgreSQL.Migrations.Application
{
    public partial class AddStripeProductsAndPrices : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PriceId",
                schema: "Billing",
                table: "StripeSubscriptions",
                type: "character varying(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "CustomerId",
                schema: "Billing",
                table: "StripeSubscriptions",
                type: "character varying(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                schema: "Billing",
                table: "StripeSubscriptions",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "StripeCustomerId",
                schema: "Billing",
                table: "Customers",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "CurrentSubscriptionId",
                schema: "Billing",
                table: "Customers",
                type: "character varying(255)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "StripeProducts",
                schema: "Billing",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Livemode = table.Column<bool>(type: "boolean", nullable: false),
                    Metadata = table.Column<string>(type: "jsonb", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StripeProducts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StripePrices",
                schema: "Billing",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    ProductId = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    UnitAmount = table.Column<long>(type: "bigint", nullable: true),
                    UnitAmountDecimal = table.Column<decimal>(type: "numeric", nullable: true),
                    Currency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    TaxBehavior = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Interval = table.Column<string>(type: "text", nullable: false),
                    IntervalCount = table.Column<long>(type: "bigint", nullable: false),
                    TrialPeriodDays = table.Column<long>(type: "bigint", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Livemode = table.Column<bool>(type: "boolean", nullable: false),
                    Metadata = table.Column<string>(type: "jsonb", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StripePrices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StripePrices_StripeProducts_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "Billing",
                        principalTable: "StripeProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StripeSubscriptions_PriceId",
                schema: "Billing",
                table: "StripeSubscriptions",
                column: "PriceId");

            migrationBuilder.CreateIndex(
                name: "IX_StripePrices_ProductId",
                schema: "Billing",
                table: "StripePrices",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_StripeSubscriptions_StripePrices_PriceId",
                schema: "Billing",
                table: "StripeSubscriptions",
                column: "PriceId",
                principalSchema: "Billing",
                principalTable: "StripePrices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StripeSubscriptions_StripePrices_PriceId",
                schema: "Billing",
                table: "StripeSubscriptions");

            migrationBuilder.DropTable(
                name: "StripePrices",
                schema: "Billing");

            migrationBuilder.DropTable(
                name: "StripeProducts",
                schema: "Billing");

            migrationBuilder.DropIndex(
                name: "IX_StripeSubscriptions_PriceId",
                schema: "Billing",
                table: "StripeSubscriptions");

            migrationBuilder.AlterColumn<string>(
                name: "PriceId",
                schema: "Billing",
                table: "StripeSubscriptions",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(255)");

            migrationBuilder.AlterColumn<string>(
                name: "CustomerId",
                schema: "Billing",
                table: "StripeSubscriptions",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(255)");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                schema: "Billing",
                table: "StripeSubscriptions",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "StripeCustomerId",
                schema: "Billing",
                table: "Customers",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "CurrentSubscriptionId",
                schema: "Billing",
                table: "Customers",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldNullable: true);
        }
    }
}
