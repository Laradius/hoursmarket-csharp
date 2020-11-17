using Microsoft.EntityFrameworkCore.Migrations;

namespace HoursMarket.Migrations
{
    public partial class MultiProject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CurrentProject",
                table: "Accounts",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "CurrentProject",
                table: "Accounts",
                type: "int",
                nullable: false,
                oldClrType: typeof(string));
        }
    }
}
