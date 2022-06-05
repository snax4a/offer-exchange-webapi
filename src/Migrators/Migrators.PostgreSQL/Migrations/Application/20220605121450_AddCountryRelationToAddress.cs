using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrators.PostgreSQL.Migrations.Application
{
    public partial class AddCountryRelationToAddress : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Address_CountryCode",
                schema: "OfferExchange",
                table: "Address",
                columns: new[] { "CountryCode", "TenantId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Address_Countries_CountryCode",
                schema: "OfferExchange",
                table: "Address",
                column: "CountryCode",
                principalSchema: "ISO",
                principalTable: "Countries",
                principalColumn: "Alpha2Code",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Address_Countries_CountryCode",
                schema: "OfferExchange",
                table: "Address");

            migrationBuilder.DropIndex(
                name: "IX_Address_CountryCode",
                schema: "OfferExchange",
                table: "Address");
        }
    }
}
