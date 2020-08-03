using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class AddSearchIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Archived",
                table: "Invoices",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "ServiceIndexs",
                columns: table => new
                {
                    ServiceName = table.Column<string>(nullable: false),
                    ServicePrice = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceIndexs", x => x.ServiceName);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ServiceIndexs");

            migrationBuilder.DropColumn(
                name: "Archived",
                table: "Invoices");
        }
    }
}
