using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Findexium.Api.Migrations
{
    /// <inheritdoc />
    public partial class RuleAndTradeUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DealType",
                table: "Trades");

            migrationBuilder.DropColumn(
                name: "Side",
                table: "Trades");

            migrationBuilder.DropColumn(
                name: "SourceListId",
                table: "Trades");

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Role",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "DealType",
                table: "Trades",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Side",
                table: "Trades",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SourceListId",
                table: "Trades",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
