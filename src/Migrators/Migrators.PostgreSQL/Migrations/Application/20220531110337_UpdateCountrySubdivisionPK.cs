using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrators.PostgreSQL.Migrations.Application
{
    public partial class UpdateCountrySubdivisionPK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CountrySubdivisions",
                schema: "ISO",
                table: "CountrySubdivisions");

            migrationBuilder.DropIndex(
                name: "IX_CountrySubdivisions_CountryAlpha2Code",
                schema: "ISO",
                table: "CountrySubdivisions");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CountrySubdivisions",
                schema: "ISO",
                table: "CountrySubdivisions",
                columns: new[] { "CountryAlpha2Code", "Name", "Code" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CountrySubdivisions",
                schema: "ISO",
                table: "CountrySubdivisions");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CountrySubdivisions",
                schema: "ISO",
                table: "CountrySubdivisions",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_CountrySubdivisions_CountryAlpha2Code",
                schema: "ISO",
                table: "CountrySubdivisions",
                column: "CountryAlpha2Code");
        }
    }
}
