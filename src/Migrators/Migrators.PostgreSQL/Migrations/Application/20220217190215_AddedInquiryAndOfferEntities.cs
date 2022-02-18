using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrators.PostgreSQL.Migrations.Application
{
    public partial class AddedInquiryAndOfferEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Inquiries",
                schema: "Catalog",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ReferenceNumber = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    Title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    TenantId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inquiries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InquiryProducts",
                schema: "Catalog",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    PreferredDeliveryDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    InquiryId = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InquiryProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InquiryProducts_Inquiries_InquiryId",
                        column: x => x.InquiryId,
                        principalSchema: "Catalog",
                        principalTable: "Inquiries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InquiryRecipients",
                schema: "Catalog",
                columns: table => new
                {
                    InquiryId = table.Column<Guid>(type: "uuid", nullable: false),
                    TraderId = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InquiryRecipients", x => new { x.InquiryId, x.TraderId });
                    table.ForeignKey(
                        name: "FK_InquiryRecipients_Inquiries_InquiryId",
                        column: x => x.InquiryId,
                        principalSchema: "Catalog",
                        principalTable: "Inquiries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InquiryRecipients_Traders_TraderId",
                        column: x => x.TraderId,
                        principalSchema: "Catalog",
                        principalTable: "Traders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Offers",
                schema: "Catalog",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CurrencyCode = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    NetValue = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    GrossValue = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    DeliveryCostType = table.Column<int>(type: "integer", nullable: false),
                    DeliveryCostPrice = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    DeliveryCostDescription = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    Freebie = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    HasFreebies = table.Column<bool>(type: "boolean", nullable: false),
                    HasReplacements = table.Column<bool>(type: "boolean", nullable: false),
                    InquiryId = table.Column<Guid>(type: "uuid", nullable: false),
                    TraderId = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Offers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Offers_Inquiries_InquiryId",
                        column: x => x.InquiryId,
                        principalSchema: "Catalog",
                        principalTable: "Inquiries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Offers_Traders_TraderId",
                        column: x => x.TraderId,
                        principalSchema: "Catalog",
                        principalTable: "Traders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OfferProducts",
                schema: "Catalog",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CurrencyCode = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    VatRate = table.Column<decimal>(type: "numeric", nullable: false),
                    NetPrice = table.Column<decimal>(type: "numeric(3,2)", nullable: false),
                    DeliveryDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    IsReplacement = table.Column<bool>(type: "boolean", nullable: false),
                    ReplacementName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Freebie = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    OfferId = table.Column<Guid>(type: "uuid", nullable: false),
                    InquiryProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfferProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OfferProducts_InquiryProducts_InquiryProductId",
                        column: x => x.InquiryProductId,
                        principalSchema: "Catalog",
                        principalTable: "InquiryProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OfferProducts_Offers_OfferId",
                        column: x => x.OfferId,
                        principalSchema: "Catalog",
                        principalTable: "Offers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Inquiries_ReferenceNumber_CreatedBy",
                schema: "Catalog",
                table: "Inquiries",
                columns: new[] { "ReferenceNumber", "CreatedBy" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InquiryProducts_InquiryId",
                schema: "Catalog",
                table: "InquiryProducts",
                column: "InquiryId");

            migrationBuilder.CreateIndex(
                name: "IX_InquiryRecipients_TraderId",
                schema: "Catalog",
                table: "InquiryRecipients",
                column: "TraderId");

            migrationBuilder.CreateIndex(
                name: "IX_OfferProducts_InquiryProductId",
                schema: "Catalog",
                table: "OfferProducts",
                column: "InquiryProductId");

            migrationBuilder.CreateIndex(
                name: "IX_OfferProducts_OfferId",
                schema: "Catalog",
                table: "OfferProducts",
                column: "OfferId");

            migrationBuilder.CreateIndex(
                name: "IX_Offers_InquiryId",
                schema: "Catalog",
                table: "Offers",
                column: "InquiryId");

            migrationBuilder.CreateIndex(
                name: "IX_Offers_TraderId",
                schema: "Catalog",
                table: "Offers",
                column: "TraderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InquiryRecipients",
                schema: "Catalog");

            migrationBuilder.DropTable(
                name: "OfferProducts",
                schema: "Catalog");

            migrationBuilder.DropTable(
                name: "InquiryProducts",
                schema: "Catalog");

            migrationBuilder.DropTable(
                name: "Offers",
                schema: "Catalog");

            migrationBuilder.DropTable(
                name: "Inquiries",
                schema: "Catalog");
        }
    }
}
