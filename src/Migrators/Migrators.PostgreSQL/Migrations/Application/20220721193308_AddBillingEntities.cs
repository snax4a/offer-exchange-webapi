using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrators.PostgreSQL.Migrations.Application
{
    public partial class AddBillingEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Billing");

            migrationBuilder.CreateTable(
                name: "Customers",
                schema: "Billing",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    StripeCustomerId = table.Column<string>(type: "text", nullable: false),
                    BillingPlan = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    MonthlyNumberOfInquiriesSent = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                    table.UniqueConstraint("AK_Customers_StripeCustomerId", x => x.StripeCustomerId);
                });

            migrationBuilder.CreateTable(
                name: "StripeSubscriptions",
                schema: "Billing",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    CustomerId = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    PriceId = table.Column<string>(type: "text", nullable: false),
                    CancelAtPeriodEnd = table.Column<bool>(type: "boolean", nullable: false),
                    CancelAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CanceledAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CollectionMethod = table.Column<string>(type: "text", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Currency = table.Column<string>(type: "text", nullable: false),
                    CurrentPeriodEnd = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CurrentPeriodStart = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    TrialStart = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    TrialEnd = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Livemode = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StripeSubscriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StripeSubscriptions_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalSchema: "Billing",
                        principalTable: "Customers",
                        principalColumn: "StripeCustomerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Customers_StripeCustomerId",
                schema: "Billing",
                table: "Customers",
                column: "StripeCustomerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customers_UserId",
                schema: "Billing",
                table: "Customers",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StripeSubscriptions_CustomerId",
                schema: "Billing",
                table: "StripeSubscriptions",
                column: "CustomerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StripeSubscriptions",
                schema: "Billing");

            migrationBuilder.DropTable(
                name: "Customers",
                schema: "Billing");
        }
    }
}
