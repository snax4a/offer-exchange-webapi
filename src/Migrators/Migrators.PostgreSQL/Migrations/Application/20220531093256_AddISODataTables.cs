using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrators.PostgreSQL.Migrations.Application
{
    public partial class AddISODataTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "ISO");

            migrationBuilder.CreateTable(
                name: "Countries",
                schema: "ISO",
                columns: table => new
                {
                    Alpha2Code = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    Alpha3Code = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    NumericCode = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CallingCodes = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: true),
                    CurrencyCode = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: true),
                    CurrencyName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    CurrencySymbol = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    Capital = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    LanguageCodes = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.Alpha2Code);
                });

            migrationBuilder.CreateTable(
                name: "CountrySubdivisions",
                schema: "ISO",
                columns: table => new
                {
                    Code = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CountryAlpha2Code = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CountrySubdivisions", x => x.Code);
                    table.ForeignKey(
                        name: "FK_CountrySubdivisions_Countries_CountryAlpha2Code",
                        column: x => x.CountryAlpha2Code,
                        principalSchema: "ISO",
                        principalTable: "Countries",
                        principalColumn: "Alpha2Code",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CountrySubdivisions_CountryAlpha2Code",
                schema: "ISO",
                table: "CountrySubdivisions",
                column: "CountryAlpha2Code");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CountrySubdivisions",
                schema: "ISO");

            migrationBuilder.DropTable(
                name: "Countries",
                schema: "ISO");
        }
    }
}
