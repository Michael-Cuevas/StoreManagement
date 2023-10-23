using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoreManagement.Migrations
{
    /// <inheritdoc />
    public partial class FIxedSalesDatumTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SalesData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MarkdownPlanId = table.Column<int>(type: "INTEGER", nullable: false),
                    SalesDate = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    TotalSold = table.Column<int>(type: "INTEGER", nullable: false)
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
                name: "IX_SalesData_MarkdownPlanId",
                table: "SalesData",
                column: "MarkdownPlanId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SalesData");
        }
    }
}
