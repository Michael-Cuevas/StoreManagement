using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoreManagement.Migrations
{
    /// <inheritdoc />
    public partial class AddedSalesData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "MarkdownPlans",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "MarkdownPlans");
        }
    }
}
