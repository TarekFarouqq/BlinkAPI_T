using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blink_API.Migrations
{
    /// <inheritdoc />
    public partial class CreateReviewSupplierProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReviewSuppliedProducts",
                columns: table => new
                {
                    RequestId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RequestStatus = table.Column<bool>(type: "bit", nullable: true),
                    ProductName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BrandId = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    SupplierId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    InventoryId = table.Column<int>(type: "int", nullable: false),
                    ProductPrice = table.Column<double>(type: "float", nullable: false),
                    ProductQuantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReviewSuppliedProducts", x => x.RequestId);
                    table.ForeignKey(
                        name: "FK_ReviewSuppliedProducts_AspNetUsers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReviewSuppliedProducts_Brands_BrandId",
                        column: x => x.BrandId,
                        principalTable: "Brands",
                        principalColumn: "BrandId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReviewSuppliedProducts_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReviewSuppliedProducts_Inventories_InventoryId",
                        column: x => x.InventoryId,
                        principalTable: "Inventories",
                        principalColumn: "InventoryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReviewSuppliedProducts_BrandId",
                table: "ReviewSuppliedProducts",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_ReviewSuppliedProducts_CategoryId",
                table: "ReviewSuppliedProducts",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ReviewSuppliedProducts_InventoryId",
                table: "ReviewSuppliedProducts",
                column: "InventoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ReviewSuppliedProducts_SupplierId",
                table: "ReviewSuppliedProducts",
                column: "SupplierId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReviewSuppliedProducts");
        }
    }
}
