using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PocketCounter.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSerialNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "serial_number",
                table: "orders",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "serial_number",
                table: "orders");
        }
    }
}
