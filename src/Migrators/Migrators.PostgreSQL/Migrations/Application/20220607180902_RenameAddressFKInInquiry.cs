using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrators.PostgreSQL.Migrations.Application
{
    public partial class RenameAddressFKInInquiry : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inquiries_Address_AddressId",
                schema: "OfferExchange",
                table: "Inquiries");

            migrationBuilder.RenameColumn(
                name: "AddressId",
                schema: "OfferExchange",
                table: "Inquiries",
                newName: "ShippingAddressId");

            migrationBuilder.RenameIndex(
                name: "IX_Inquiries_AddressId",
                schema: "OfferExchange",
                table: "Inquiries",
                newName: "IX_Inquiries_ShippingAddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_Inquiries_Address_ShippingAddressId",
                schema: "OfferExchange",
                table: "Inquiries",
                column: "ShippingAddressId",
                principalSchema: "OfferExchange",
                principalTable: "Address",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inquiries_Address_ShippingAddressId",
                schema: "OfferExchange",
                table: "Inquiries");

            migrationBuilder.RenameColumn(
                name: "ShippingAddressId",
                schema: "OfferExchange",
                table: "Inquiries",
                newName: "AddressId");

            migrationBuilder.RenameIndex(
                name: "IX_Inquiries_ShippingAddressId",
                schema: "OfferExchange",
                table: "Inquiries",
                newName: "IX_Inquiries_AddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_Inquiries_Address_AddressId",
                schema: "OfferExchange",
                table: "Inquiries",
                column: "AddressId",
                principalSchema: "OfferExchange",
                principalTable: "Address",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
