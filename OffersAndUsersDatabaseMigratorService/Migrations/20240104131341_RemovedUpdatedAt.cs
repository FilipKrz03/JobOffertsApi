using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OffersAndUsersDatabaseMigratorService.Migrations
{
    public partial class RemovedUpdatedAt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Technologies");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "JobOffers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Users",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Technologies",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "JobOffers",
                type: "datetime2",
                nullable: true);
        }
    }
}
