using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrators.PostgreSQL.Migrations.Application
{
    public partial class UpdatedOfferEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "Catalog",
                table: "Offers");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                schema: "Catalog",
                table: "Offers");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "Catalog",
                table: "Offers");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                schema: "Catalog",
                table: "Offers");

            migrationBuilder.DropColumn(
                name: "LastModifiedOn",
                schema: "Catalog",
                table: "Offers");

            migrationBuilder.RenameColumn(
                name: "LastModifiedBy",
                schema: "Catalog",
                table: "Offers",
                newName: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Offers_UserId",
                schema: "Catalog",
                table: "Offers",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Offers_UserId",
                schema: "Catalog",
                table: "Offers");

            migrationBuilder.RenameColumn(
                name: "UserId",
                schema: "Catalog",
                table: "Offers",
                newName: "LastModifiedBy");

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                schema: "Catalog",
                table: "Offers",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                schema: "Catalog",
                table: "Offers",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                schema: "Catalog",
                table: "Offers",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                schema: "Catalog",
                table: "Offers",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedOn",
                schema: "Catalog",
                table: "Offers",
                type: "timestamp without time zone",
                nullable: true);
        }
    }
}
