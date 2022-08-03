using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrators.PostgreSQL.Migrations.Application
{
    public partial class AddCurrentSubscriptionToCustomer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CurrentSubscriptionId",
                schema: "Billing",
                table: "Customers",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customers_CurrentSubscriptionId",
                schema: "Billing",
                table: "Customers",
                column: "CurrentSubscriptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_StripeSubscriptions_CurrentSubscriptionId",
                schema: "Billing",
                table: "Customers",
                column: "CurrentSubscriptionId",
                principalSchema: "Billing",
                principalTable: "StripeSubscriptions",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_StripeSubscriptions_CurrentSubscriptionId",
                schema: "Billing",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_Customers_CurrentSubscriptionId",
                schema: "Billing",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "CurrentSubscriptionId",
                schema: "Billing",
                table: "Customers");
        }
    }
}
