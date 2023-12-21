using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobOffersService.Migrations
{
    public partial class AddedFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EarningsFrom",
                table: "JobOffers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EarningsTo",
                table: "JobOffers",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EarningsFrom",
                table: "JobOffers");

            migrationBuilder.DropColumn(
                name: "EarningsTo",
                table: "JobOffers");
        }
    }
}
