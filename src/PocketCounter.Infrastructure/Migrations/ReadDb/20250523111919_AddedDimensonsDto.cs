using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PocketCounter.Infrastructure.Migrations.ReadDb
{
    /// <inheritdoc />
    public partial class AddedDimensonsDto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "depth",
                table: "products");

            migrationBuilder.DropColumn(
                name: "height",
                table: "products");

            migrationBuilder.DropColumn(
                name: "width",
                table: "products");

            migrationBuilder.AddColumn<string>(
                name: "dimensions",
                table: "products",
                type: "jsonb",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "dimensions",
                table: "products");

            migrationBuilder.AddColumn<double>(
                name: "depth",
                table: "products",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "height",
                table: "products",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "width",
                table: "products",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
