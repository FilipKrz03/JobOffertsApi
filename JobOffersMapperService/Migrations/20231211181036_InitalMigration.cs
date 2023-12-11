using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobOffersMapperService.Migrations
{
    public partial class InitalMigration : Migration
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
                    OfferLink = table.Column<string>(type: "nvarchar(max)", nullable: false)
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
