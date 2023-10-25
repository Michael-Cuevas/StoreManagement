using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoreManagement.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Upc = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Inventories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inventories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Inventories_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MarkdownPlans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InitialInventory = table.Column<int>(type: "int", nullable: false),
                    CurrentSaleDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    InitialReduction = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IntermidiateReduction = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FinalReduction = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    SaleEnded = table.Column<bool>(type: "bit", nullable: false),
                    IntermediateCompleted = table.Column<bool>(type: "bit", nullable: false),
                    InitialCompleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MarkdownPlans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MarkdownPlans_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SalesData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MarkdownPlanId = table.Column<int>(type: "int", nullable: false),
                    SalesDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Margin = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalSold = table.Column<int>(type: "int", nullable: false),
                    TotalProfit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RemainingInventory = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalesData_MarkdownPlans_MarkdownPlanId",
                        column: x => x.MarkdownPlanId,
                        principalTable: "MarkdownPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Inventories_ProductId",
                table: "Inventories",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_MarkdownPlans_ProductId",
                table: "MarkdownPlans",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesData_MarkdownPlanId",
                table: "SalesData",
                column: "MarkdownPlanId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Inventories");

            migrationBuilder.DropTable(
                name: "SalesData");

            migrationBuilder.DropTable(
                name: "MarkdownPlans");

            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
