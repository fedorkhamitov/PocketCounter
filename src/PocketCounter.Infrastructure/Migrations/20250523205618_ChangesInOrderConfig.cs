using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PocketCounter.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangesInOrderConfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "cartLines",
                table: "orders",
                newName: "cart_lines");

            migrationBuilder.AlterColumn<Guid>(
                name: "customer_id",
                table: "orders",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "cart_lines",
                table: "orders",
                newName: "cartLines");

            migrationBuilder.AlterColumn<Guid>(
                name: "customer_id",
                table: "orders",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);
        }
    }
}
