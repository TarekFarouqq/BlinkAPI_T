using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blink_API.Migrations
{
    /// <inheritdoc />
    public partial class CreateReviewSupplierProductsP2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReviewSuppliedProductImages",
                columns: table => new
                {
                    RequestId = table.Column<int>(type: "int", nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReviewSuppliedProductImages", x => new { x.RequestId, x.ImagePath });
                    table.ForeignKey(
                        name: "FK_ReviewSuppliedProductImages_ReviewSuppliedProducts_RequestId",
                        column: x => x.RequestId,
                        principalTable: "ReviewSuppliedProducts",
                        principalColumn: "RequestId",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReviewSuppliedProductImages");
        }
    }
}
