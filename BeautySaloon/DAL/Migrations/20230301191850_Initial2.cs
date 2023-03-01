using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BeautySaloon.DAL.Migrations
{
    public partial class Initial2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PersonSubscription_Appointment_OrderId",
                table: "PersonSubscription");

            migrationBuilder.DropColumn(
                name: "Comment",
                table: "Person");

            migrationBuilder.CreateIndex(
                name: "IX_PersonSubscription_AppointmentId",
                table: "PersonSubscription",
                column: "AppointmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_PersonSubscription_Appointment_AppointmentId",
                table: "PersonSubscription",
                column: "AppointmentId",
                principalTable: "Appointment",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PersonSubscription_Appointment_AppointmentId",
                table: "PersonSubscription");

            migrationBuilder.DropIndex(
                name: "IX_PersonSubscription_AppointmentId",
                table: "PersonSubscription");

            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "Person",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonSubscription_Appointment_OrderId",
                table: "PersonSubscription",
                column: "OrderId",
                principalTable: "Appointment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
