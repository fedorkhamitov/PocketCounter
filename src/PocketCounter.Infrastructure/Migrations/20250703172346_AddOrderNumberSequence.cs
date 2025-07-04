using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PocketCounter.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderNumberSequence : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "serial_number",
                table: "orders");

            migrationBuilder.CreateSequence<int>(
                name: "OrderNumbers");

            migrationBuilder.AddColumn<int>(
                name: "order_number",
                table: "orders",
                type: "integer",
                nullable: false,
                defaultValueSql: "nextval('\"OrderNumbers\"')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "order_number",
                table: "orders");

            migrationBuilder.DropSequence(
                name: "OrderNumbers");

            migrationBuilder.AddColumn<int>(
                name: "serial_number",
                table: "orders",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
