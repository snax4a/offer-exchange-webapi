using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrators.PostgreSQL.Migrations.Application
{
    public partial class UpdateRelationName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAddress_Address_AddressId",
                schema: "OfferExchange",
                table: "UserAddress");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserAddress",
                schema: "OfferExchange",
                table: "UserAddress");

            migrationBuilder.RenameTable(
                name: "UserAddress",
                schema: "OfferExchange",
                newName: "UserAddresses",
                newSchema: "OfferExchange");

            migrationBuilder.RenameIndex(
                name: "IX_UserAddress_AddressId",
                schema: "OfferExchange",
                table: "UserAddresses",
                newName: "IX_UserAddresses_AddressId");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "OfferExchange",
                table: "UserAddresses",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserAddresses",
                schema: "OfferExchange",
                table: "UserAddresses",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_UserAddresses_CreatedBy_Name",
                schema: "OfferExchange",
                table: "UserAddresses",
                columns: new[] { "CreatedBy", "Name", "TenantId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UserAddresses_Address_AddressId",
                schema: "OfferExchange",
                table: "UserAddresses",
                column: "AddressId",
                principalSchema: "OfferExchange",
                principalTable: "Address",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAddresses_Address_AddressId",
                schema: "OfferExchange",
                table: "UserAddresses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserAddresses",
                schema: "OfferExchange",
                table: "UserAddresses");

            migrationBuilder.DropIndex(
                name: "IX_UserAddresses_CreatedBy_Name",
                schema: "OfferExchange",
                table: "UserAddresses");

            migrationBuilder.RenameTable(
                name: "UserAddresses",
                schema: "OfferExchange",
                newName: "UserAddress",
                newSchema: "OfferExchange");

            migrationBuilder.RenameIndex(
                name: "IX_UserAddresses_AddressId",
                schema: "OfferExchange",
                table: "UserAddress",
                newName: "IX_UserAddress_AddressId");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "OfferExchange",
                table: "UserAddress",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserAddress",
                schema: "OfferExchange",
                table: "UserAddress",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAddress_Address_AddressId",
                schema: "OfferExchange",
                table: "UserAddress",
                column: "AddressId",
                principalSchema: "OfferExchange",
                principalTable: "Address",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
