using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrators.PostgreSQL.Migrations.Application
{
    public partial class UpdateSchemaName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "OfferExchange");

            migrationBuilder.RenameTable(
                name: "Traders",
                schema: "Catalog",
                newName: "Traders",
                newSchema: "OfferExchange");

            migrationBuilder.RenameTable(
                name: "TraderGroup",
                schema: "Catalog",
                newName: "TraderGroup",
                newSchema: "OfferExchange");

            migrationBuilder.RenameTable(
                name: "Orders",
                schema: "Catalog",
                newName: "Orders",
                newSchema: "OfferExchange");

            migrationBuilder.RenameTable(
                name: "OrderProducts",
                schema: "Catalog",
                newName: "OrderProducts",
                newSchema: "OfferExchange");

            migrationBuilder.RenameTable(
                name: "Offers",
                schema: "Catalog",
                newName: "Offers",
                newSchema: "OfferExchange");

            migrationBuilder.RenameTable(
                name: "OfferProducts",
                schema: "Catalog",
                newName: "OfferProducts",
                newSchema: "OfferExchange");

            migrationBuilder.RenameTable(
                name: "InquiryRecipients",
                schema: "Catalog",
                newName: "InquiryRecipients",
                newSchema: "OfferExchange");

            migrationBuilder.RenameTable(
                name: "InquiryProducts",
                schema: "Catalog",
                newName: "InquiryProducts",
                newSchema: "OfferExchange");

            migrationBuilder.RenameTable(
                name: "Inquiries",
                schema: "Catalog",
                newName: "Inquiries",
                newSchema: "OfferExchange");

            migrationBuilder.RenameTable(
                name: "Groups",
                schema: "Catalog",
                newName: "Groups",
                newSchema: "OfferExchange");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Catalog");

            migrationBuilder.RenameTable(
                name: "Traders",
                schema: "OfferExchange",
                newName: "Traders",
                newSchema: "Catalog");

            migrationBuilder.RenameTable(
                name: "TraderGroup",
                schema: "OfferExchange",
                newName: "TraderGroup",
                newSchema: "Catalog");

            migrationBuilder.RenameTable(
                name: "Orders",
                schema: "OfferExchange",
                newName: "Orders",
                newSchema: "Catalog");

            migrationBuilder.RenameTable(
                name: "OrderProducts",
                schema: "OfferExchange",
                newName: "OrderProducts",
                newSchema: "Catalog");

            migrationBuilder.RenameTable(
                name: "Offers",
                schema: "OfferExchange",
                newName: "Offers",
                newSchema: "Catalog");

            migrationBuilder.RenameTable(
                name: "OfferProducts",
                schema: "OfferExchange",
                newName: "OfferProducts",
                newSchema: "Catalog");

            migrationBuilder.RenameTable(
                name: "InquiryRecipients",
                schema: "OfferExchange",
                newName: "InquiryRecipients",
                newSchema: "Catalog");

            migrationBuilder.RenameTable(
                name: "InquiryProducts",
                schema: "OfferExchange",
                newName: "InquiryProducts",
                newSchema: "Catalog");

            migrationBuilder.RenameTable(
                name: "Inquiries",
                schema: "OfferExchange",
                newName: "Inquiries",
                newSchema: "Catalog");

            migrationBuilder.RenameTable(
                name: "Groups",
                schema: "OfferExchange",
                newName: "Groups",
                newSchema: "Catalog");
        }
    }
}
