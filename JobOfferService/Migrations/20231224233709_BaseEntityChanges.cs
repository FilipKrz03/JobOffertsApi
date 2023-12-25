using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobOffersService.Migrations
{
    public partial class BaseEntityChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Technologies",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Technologies",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "JobOffers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "JobOffers",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Technologies");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Technologies");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "JobOffers");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "JobOffers");
        }
    }
}
