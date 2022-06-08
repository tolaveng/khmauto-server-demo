using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class Service_Price_backup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "ServicePriceOld",
                table: "Services",
                type: "decimal(8, 2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ServicePriceOld",
                table: "Services");
        }
    }
}
