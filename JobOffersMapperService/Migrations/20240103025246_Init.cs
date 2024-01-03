using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobOffersMapperService.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BaseJobOffer",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OfferTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OfferCompany = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OfferLink = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaseJobOffer", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BaseJobOffer");
        }
    }
}
