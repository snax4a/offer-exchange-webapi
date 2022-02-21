using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrators.PostgreSQL.Migrations.Application
{
    public partial class AddExpirationDateToOffer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ExpirationDate",
                schema: "Catalog",
                table: "Offers",
                type: "timestamp without time zone",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpirationDate",
                schema: "Catalog",
                table: "Offers");
        }
    }
}
