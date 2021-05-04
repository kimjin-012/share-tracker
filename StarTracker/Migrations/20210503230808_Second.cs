using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace StarTracker.Migrations
{
    public partial class Second : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Stars");

            migrationBuilder.AddColumn<DateTime>(
                name: "Time",
                table: "Stars",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Time",
                table: "Stars");

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Stars",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);
        }
    }
}
