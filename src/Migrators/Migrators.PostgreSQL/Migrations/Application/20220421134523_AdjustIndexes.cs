using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrators.PostgreSQL.Migrations.Application
{
    public partial class AdjustIndexes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TraderGroup_GroupId",
                schema: "OfferExchange",
                table: "TraderGroup");

            migrationBuilder.DropIndex(
                name: "IX_Orders_TraderId",
                schema: "OfferExchange",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_OrderProducts_OfferProductId",
                schema: "OfferExchange",
                table: "OrderProducts");

            migrationBuilder.DropIndex(
                name: "IX_Offers_InquiryId",
                schema: "OfferExchange",
                table: "Offers");

            migrationBuilder.DropIndex(
                name: "IX_Offers_TraderId",
                schema: "OfferExchange",
                table: "Offers");

            migrationBuilder.DropIndex(
                name: "IX_Offers_UserId",
                schema: "OfferExchange",
                table: "Offers");

            migrationBuilder.DropIndex(
                name: "IX_OfferProducts_InquiryProductId",
                schema: "OfferExchange",
                table: "OfferProducts");

            migrationBuilder.DropIndex(
                name: "IX_OfferProducts_OfferId",
                schema: "OfferExchange",
                table: "OfferProducts");

            migrationBuilder.DropIndex(
                name: "IX_InquiryRecipients_TraderId",
                schema: "OfferExchange",
                table: "InquiryRecipients");

            migrationBuilder.DropIndex(
                name: "IX_InquiryProducts_InquiryId",
                schema: "OfferExchange",
                table: "InquiryProducts");

            migrationBuilder.DropIndex(
                name: "IX_Inquiries_ReferenceNumber_CreatedBy",
                schema: "OfferExchange",
                table: "Inquiries");

            migrationBuilder.DropIndex(
                name: "IX_Groups_Name_CreatedBy",
                schema: "OfferExchange",
                table: "Groups");

            migrationBuilder.CreateIndex(
                name: "IX_Traders_CreatedBy",
                schema: "OfferExchange",
                table: "Traders",
                columns: new[] { "CreatedBy", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_TraderGroup_GroupId",
                schema: "OfferExchange",
                table: "TraderGroup",
                columns: new[] { "GroupId", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CreatedBy",
                schema: "OfferExchange",
                table: "Orders",
                columns: new[] { "CreatedBy", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_TraderId",
                schema: "OfferExchange",
                table: "Orders",
                columns: new[] { "TraderId", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_OrderProducts_OfferProductId",
                schema: "OfferExchange",
                table: "OrderProducts",
                columns: new[] { "OfferProductId", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_Offers_CreatedOn",
                schema: "OfferExchange",
                table: "Offers",
                columns: new[] { "CreatedOn", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_Offers_InquiryId",
                schema: "OfferExchange",
                table: "Offers",
                columns: new[] { "InquiryId", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_Offers_TraderId",
                schema: "OfferExchange",
                table: "Offers",
                columns: new[] { "TraderId", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_Offers_UserId",
                schema: "OfferExchange",
                table: "Offers",
                columns: new[] { "UserId", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_OfferProducts_InquiryProductId",
                schema: "OfferExchange",
                table: "OfferProducts",
                columns: new[] { "InquiryProductId", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_OfferProducts_OfferId",
                schema: "OfferExchange",
                table: "OfferProducts",
                columns: new[] { "OfferId", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_InquiryRecipients_TraderId",
                schema: "OfferExchange",
                table: "InquiryRecipients",
                columns: new[] { "TraderId", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_InquiryProducts_CreatedBy",
                schema: "OfferExchange",
                table: "InquiryProducts",
                columns: new[] { "CreatedBy", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_InquiryProducts_InquiryId",
                schema: "OfferExchange",
                table: "InquiryProducts",
                columns: new[] { "InquiryId", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_Inquiries_ReferenceNumber_CreatedBy",
                schema: "OfferExchange",
                table: "Inquiries",
                columns: new[] { "ReferenceNumber", "CreatedBy", "TenantId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Groups_Name_CreatedBy",
                schema: "OfferExchange",
                table: "Groups",
                columns: new[] { "Name", "CreatedBy", "TenantId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Traders_CreatedBy",
                schema: "OfferExchange",
                table: "Traders");

            migrationBuilder.DropIndex(
                name: "IX_TraderGroup_GroupId",
                schema: "OfferExchange",
                table: "TraderGroup");

            migrationBuilder.DropIndex(
                name: "IX_Orders_CreatedBy",
                schema: "OfferExchange",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_TraderId",
                schema: "OfferExchange",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_OrderProducts_OfferProductId",
                schema: "OfferExchange",
                table: "OrderProducts");

            migrationBuilder.DropIndex(
                name: "IX_Offers_CreatedOn",
                schema: "OfferExchange",
                table: "Offers");

            migrationBuilder.DropIndex(
                name: "IX_Offers_InquiryId",
                schema: "OfferExchange",
                table: "Offers");

            migrationBuilder.DropIndex(
                name: "IX_Offers_TraderId",
                schema: "OfferExchange",
                table: "Offers");

            migrationBuilder.DropIndex(
                name: "IX_Offers_UserId",
                schema: "OfferExchange",
                table: "Offers");

            migrationBuilder.DropIndex(
                name: "IX_OfferProducts_InquiryProductId",
                schema: "OfferExchange",
                table: "OfferProducts");

            migrationBuilder.DropIndex(
                name: "IX_OfferProducts_OfferId",
                schema: "OfferExchange",
                table: "OfferProducts");

            migrationBuilder.DropIndex(
                name: "IX_InquiryRecipients_TraderId",
                schema: "OfferExchange",
                table: "InquiryRecipients");

            migrationBuilder.DropIndex(
                name: "IX_InquiryProducts_CreatedBy",
                schema: "OfferExchange",
                table: "InquiryProducts");

            migrationBuilder.DropIndex(
                name: "IX_InquiryProducts_InquiryId",
                schema: "OfferExchange",
                table: "InquiryProducts");

            migrationBuilder.DropIndex(
                name: "IX_Inquiries_ReferenceNumber_CreatedBy",
                schema: "OfferExchange",
                table: "Inquiries");

            migrationBuilder.DropIndex(
                name: "IX_Groups_Name_CreatedBy",
                schema: "OfferExchange",
                table: "Groups");

            migrationBuilder.CreateIndex(
                name: "IX_TraderGroup_GroupId",
                schema: "OfferExchange",
                table: "TraderGroup",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_TraderId",
                schema: "OfferExchange",
                table: "Orders",
                column: "TraderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderProducts_OfferProductId",
                schema: "OfferExchange",
                table: "OrderProducts",
                column: "OfferProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Offers_InquiryId",
                schema: "OfferExchange",
                table: "Offers",
                column: "InquiryId");

            migrationBuilder.CreateIndex(
                name: "IX_Offers_TraderId",
                schema: "OfferExchange",
                table: "Offers",
                column: "TraderId");

            migrationBuilder.CreateIndex(
                name: "IX_Offers_UserId",
                schema: "OfferExchange",
                table: "Offers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_OfferProducts_InquiryProductId",
                schema: "OfferExchange",
                table: "OfferProducts",
                column: "InquiryProductId");

            migrationBuilder.CreateIndex(
                name: "IX_OfferProducts_OfferId",
                schema: "OfferExchange",
                table: "OfferProducts",
                column: "OfferId");

            migrationBuilder.CreateIndex(
                name: "IX_InquiryRecipients_TraderId",
                schema: "OfferExchange",
                table: "InquiryRecipients",
                column: "TraderId");

            migrationBuilder.CreateIndex(
                name: "IX_InquiryProducts_InquiryId",
                schema: "OfferExchange",
                table: "InquiryProducts",
                column: "InquiryId");

            migrationBuilder.CreateIndex(
                name: "IX_Inquiries_ReferenceNumber_CreatedBy",
                schema: "OfferExchange",
                table: "Inquiries",
                columns: new[] { "ReferenceNumber", "CreatedBy" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Groups_Name_CreatedBy",
                schema: "OfferExchange",
                table: "Groups",
                columns: new[] { "Name", "CreatedBy" });
        }
    }
}
