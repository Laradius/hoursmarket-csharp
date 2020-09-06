using Microsoft.EntityFrameworkCore.Migrations;

namespace HoursMarket.Migrations
{
    public partial class AccountIdNotNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HourOffers_Accounts_AccountId",
                table: "HourOffers");

            migrationBuilder.AlterColumn<int>(
                name: "AccountId",
                table: "HourOffers",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_HourOffers_Accounts_AccountId",
                table: "HourOffers",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HourOffers_Accounts_AccountId",
                table: "HourOffers");

            migrationBuilder.AlterColumn<int>(
                name: "AccountId",
                table: "HourOffers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_HourOffers_Accounts_AccountId",
                table: "HourOffers",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
