using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BeautySaloon.DAL.Migrations
{
    public partial class kek : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubscriptionCosmeticService_Subscription_SubscriptionId1",
                table: "SubscriptionCosmeticService");

            migrationBuilder.DropIndex(
                name: "IX_SubscriptionCosmeticService_SubscriptionId1",
                table: "SubscriptionCosmeticService");

            migrationBuilder.DropColumn(
                name: "SubscriptionId1",
                table: "SubscriptionCosmeticService");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SubscriptionId1",
                table: "SubscriptionCosmeticService",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_SubscriptionCosmeticService_SubscriptionId1",
                table: "SubscriptionCosmeticService",
                column: "SubscriptionId1");

            migrationBuilder.AddForeignKey(
                name: "FK_SubscriptionCosmeticService_Subscription_SubscriptionId1",
                table: "SubscriptionCosmeticService",
                column: "SubscriptionId1",
                principalTable: "Subscription",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
