using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrators.PostgreSQL.Migrations.Application
{
    public partial class AddShippingAddressToOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ShippingAddressId",
                schema: "OfferExchange",
                table: "Orders",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ShippingAddressId",
                schema: "OfferExchange",
                table: "Orders",
                columns: new[] { "ShippingAddressId", "TenantId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Address_ShippingAddressId",
                schema: "OfferExchange",
                table: "Orders",
                column: "ShippingAddressId",
                principalSchema: "OfferExchange",
                principalTable: "Address",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Address_ShippingAddressId",
                schema: "OfferExchange",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_ShippingAddressId",
                schema: "OfferExchange",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ShippingAddressId",
                schema: "OfferExchange",
                table: "Orders");
        }
    }
}
