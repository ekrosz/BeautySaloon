using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BeautySaloon.DAL.Migrations
{
    public partial class lol : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_Person_PersonId1",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_PersonId1",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "PersonId1",
                table: "Order");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PersonId1",
                table: "Order",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Order_PersonId1",
                table: "Order",
                column: "PersonId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Person_PersonId1",
                table: "Order",
                column: "PersonId1",
                principalTable: "Person",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
