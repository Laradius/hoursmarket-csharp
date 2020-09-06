using Microsoft.EntityFrameworkCore.Migrations;

namespace HoursMarket.Migrations
{
    public partial class TableUpdateCurrentProject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CurrentProject",
                table: "Accounts",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentProject",
                table: "Accounts");
        }
    }
}
