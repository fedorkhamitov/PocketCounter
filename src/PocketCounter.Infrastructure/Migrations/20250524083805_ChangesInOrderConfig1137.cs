﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PocketCounter.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangesInOrderConfig1137 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "customer_id",
                table: "orders",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");
        }
    }
}
