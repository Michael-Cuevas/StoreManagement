using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoreManagement.Migrations
{
    /// <inheritdoc />
    public partial class NewFieldsToMarkdown : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "InitialCompleted",
                table: "MarkdownPlans",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IntermediateCompleted",
                table: "MarkdownPlans",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SaleEnded",
                table: "MarkdownPlans",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InitialCompleted",
                table: "MarkdownPlans");

            migrationBuilder.DropColumn(
                name: "IntermediateCompleted",
                table: "MarkdownPlans");

            migrationBuilder.DropColumn(
                name: "SaleEnded",
                table: "MarkdownPlans");
        }
    }
}
