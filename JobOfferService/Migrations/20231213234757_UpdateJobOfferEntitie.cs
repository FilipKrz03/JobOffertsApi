using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobOffersService.Migrations
{
    public partial class UpdateJobOfferEntitie : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OfferLink",
                table: "JobOffers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OfferLink",
                table: "JobOffers");
        }
    }
}
