using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Findexium.Api.Migrations
{
    /// <inheritdoc />
    public partial class BoolToInt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "OrderNumber",
                table: "Ratings",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte>(
                name: "OrderNumber",
                table: "Ratings",
                type: "tinyint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
