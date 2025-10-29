using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OsrsTool.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BuyPrice",
                table: "Entities");

            migrationBuilder.DropColumn(
                name: "SellPrice",
                table: "Entities");

            migrationBuilder.AddColumn<string>(
                name: "Examine",
                table: "Entities",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "HighPrice",
                table: "Entities",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Limit",
                table: "Entities",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LowPrice",
                table: "Entities",
                type: "INTEGER",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Examine",
                table: "Entities");

            migrationBuilder.DropColumn(
                name: "HighPrice",
                table: "Entities");

            migrationBuilder.DropColumn(
                name: "Limit",
                table: "Entities");

            migrationBuilder.DropColumn(
                name: "LowPrice",
                table: "Entities");

            migrationBuilder.AddColumn<long>(
                name: "BuyPrice",
                table: "Entities",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "SellPrice",
                table: "Entities",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
