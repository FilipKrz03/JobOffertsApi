using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OffersAndUsersDatabaseMigratorService.Migrations
{
    public partial class AddedDesiredSeniorityFieldOnUserEntitie : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DesiredSeniority",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DesiredSeniority",
                table: "Users");
        }
    }
}
