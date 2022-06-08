using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrators.PostgreSQL.Migrations.Application
{
    public partial class AddAddressToOffer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ShippingAddressId",
                schema: "OfferExchange",
                table: "Offers",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Offers_ShippingAddressId",
                schema: "OfferExchange",
                table: "Offers",
                columns: new[] { "ShippingAddressId", "TenantId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Offers_Address_ShippingAddressId",
                schema: "OfferExchange",
                table: "Offers",
                column: "ShippingAddressId",
                principalSchema: "OfferExchange",
                principalTable: "Address",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Offers_Address_ShippingAddressId",
                schema: "OfferExchange",
                table: "Offers");

            migrationBuilder.DropIndex(
                name: "IX_Offers_ShippingAddressId",
                schema: "OfferExchange",
                table: "Offers");

            migrationBuilder.DropColumn(
                name: "ShippingAddressId",
                schema: "OfferExchange",
                table: "Offers");
        }
    }
}
