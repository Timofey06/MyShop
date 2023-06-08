﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyShop_DataMigrations
{
    public partial class AddColumnsToOrderHeaderTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateExecution",
                table: "OrderHeader",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "TransactionId",
                table: "OrderHeader",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateExecution",
                table: "OrderHeader");

            migrationBuilder.DropColumn(
                name: "TransactionId",
                table: "OrderHeader");
        }
    }
}
