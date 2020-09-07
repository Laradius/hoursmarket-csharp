using Microsoft.EntityFrameworkCore.Migrations;

namespace HoursMarket.Migrations
{
    public partial class HourOfferProjectAdd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Project",
                table: "HourOffers",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Project",
                table: "HourOffers");
        }
    }
}
