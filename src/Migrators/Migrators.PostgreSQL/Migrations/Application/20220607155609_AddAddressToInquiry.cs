using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrators.PostgreSQL.Migrations.Application
{
    public partial class AddAddressToInquiry : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AddressId",
                schema: "OfferExchange",
                table: "Inquiries",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Inquiries_AddressId",
                schema: "OfferExchange",
                table: "Inquiries",
                columns: new[] { "AddressId", "TenantId" });

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inquiries_Address_AddressId",
                schema: "OfferExchange",
                table: "Inquiries");

            migrationBuilder.DropIndex(
                name: "IX_Inquiries_AddressId",
                schema: "OfferExchange",
                table: "Inquiries");

            migrationBuilder.DropColumn(
                name: "AddressId",
                schema: "OfferExchange",
                table: "Inquiries");
        }
    }
}
