using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class RemoveRelationsAndFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Parts_Cars_CarId",
                table: "Parts");

            migrationBuilder.DropIndex(
                name: "IX_Parts_CarId",
                table: "Parts");

            migrationBuilder.DropColumn(
                name: "CarId",
                table: "Parts");

            migrationBuilder.DropColumn(
                name: "Sku",
                table: "Parts");

            migrationBuilder.DropColumn(
                name: "StockQuantity",
                table: "Parts");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CarId",
                table: "Parts",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Sku",
                table: "Parts",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "StockQuantity",
                table: "Parts",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Parts_CarId",
                table: "Parts",
                column: "CarId");

            migrationBuilder.AddForeignKey(
                name: "FK_Parts_Cars_CarId",
                table: "Parts",
                column: "CarId",
                principalTable: "Cars",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
