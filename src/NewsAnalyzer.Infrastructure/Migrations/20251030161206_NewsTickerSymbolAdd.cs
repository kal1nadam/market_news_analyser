using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewsAnalyzer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class NewsTickerSymbolAdd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TickerSymbol",
                table: "news",
                type: "TEXT",
                maxLength: 10,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TickerSymbol",
                table: "news");
        }
    }
}
