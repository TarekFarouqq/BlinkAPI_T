using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blink_API.Migrations
{
    /// <inheritdoc />
    public partial class AddingLatandLong : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Lat",
                table: "Inventories",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Long",
                table: "Inventories",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Lat",
                table: "Inventories");

            migrationBuilder.DropColumn(
                name: "Long",
                table: "Inventories");
        }
    }
}
