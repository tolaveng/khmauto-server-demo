using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class decimalformat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Quotes_Cars_CarNo1",
                table: "Quotes");

            migrationBuilder.DropIndex(
                name: "IX_Quotes_CarNo1",
                table: "Quotes");

            migrationBuilder.DropColumn(
                name: "CarNo1",
                table: "Quotes");

            migrationBuilder.AlterColumn<decimal>(
                name: "ServicePrice",
                table: "Services",
                type: "decimal(8, 2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");

            migrationBuilder.AlterColumn<decimal>(
                name: "ServicePrice",
                table: "ServiceIndexs",
                type: "decimal(8, 2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");

            migrationBuilder.AlterColumn<string>(
                name: "CarNo",
                table: "Quotes",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Quotes_CarNo",
                table: "Quotes",
                column: "CarNo");

            migrationBuilder.AddForeignKey(
                name: "FK_Quotes_Cars_CarNo",
                table: "Quotes",
                column: "CarNo",
                principalTable: "Cars",
                principalColumn: "CarNo",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Quotes_Cars_CarNo",
                table: "Quotes");

            migrationBuilder.DropIndex(
                name: "IX_Quotes_CarNo",
                table: "Quotes");

            migrationBuilder.AlterColumn<decimal>(
                name: "ServicePrice",
                table: "Services",
                type: "decimal(65,30)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(8, 2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "ServicePrice",
                table: "ServiceIndexs",
                type: "decimal(65,30)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(8, 2)");

            migrationBuilder.AlterColumn<string>(
                name: "CarNo",
                table: "Quotes",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CarNo1",
                table: "Quotes",
                type: "varchar(255) CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Quotes_CarNo1",
                table: "Quotes",
                column: "CarNo1");

            migrationBuilder.AddForeignKey(
                name: "FK_Quotes_Cars_CarNo1",
                table: "Quotes",
                column: "CarNo1",
                principalTable: "Cars",
                principalColumn: "CarNo",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
