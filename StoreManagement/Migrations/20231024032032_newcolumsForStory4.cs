using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoreManagement.Migrations
{
    /// <inheritdoc />
    public partial class newcolumsForStory4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Margin",
                table: "SalesData",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RemainingInventory",
                table: "SalesData",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalProfit",
                table: "SalesData",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "InitialInventory",
                table: "MarkdownPlans",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Margin",
                table: "SalesData");

            migrationBuilder.DropColumn(
                name: "RemainingInventory",
                table: "SalesData");

            migrationBuilder.DropColumn(
                name: "TotalProfit",
                table: "SalesData");

            migrationBuilder.DropColumn(
                name: "InitialInventory",
                table: "MarkdownPlans");
        }
    }
}
