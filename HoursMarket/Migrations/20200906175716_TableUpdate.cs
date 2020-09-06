using Microsoft.EntityFrameworkCore.Migrations;

namespace HoursMarket.Migrations
{
    public partial class TableUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AccountId",
                table: "HourOffers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_HourOffers_AccountId",
                table: "HourOffers",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_HourOffers_Accounts_AccountId",
                table: "HourOffers",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HourOffers_Accounts_AccountId",
                table: "HourOffers");

            migrationBuilder.DropIndex(
                name: "IX_HourOffers_AccountId",
                table: "HourOffers");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "HourOffers");
        }
    }
}
