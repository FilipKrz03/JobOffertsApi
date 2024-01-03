﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OffersAndUsersDatabaseMigratorService.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "JobOffers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OfferTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OfferCompany = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Localization = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WorkMode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OfferLink = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Seniority = table.Column<int>(type: "int", nullable: false),
                    EarningsFrom = table.Column<int>(type: "int", nullable: true),
                    EarningsTo = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobOffers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Technologies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TechnologyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Technologies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdentityId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            //migrationBuilder.CreateTable(
            //    name: "JobOfferTechnology",
            //    columns: table => new
            //    {
            //        JobOffersId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        TechnologiesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_JobOfferTechnology", x => new { x.JobOffersId, x.TechnologiesId });
            //        table.ForeignKey(
            //            name: "FK_JobOfferTechnology_JobOffers_JobOffersId",
            //            column: x => x.JobOffersId,
            //            principalTable: "JobOffers",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_JobOfferTechnology_Technologies_TechnologiesId",
            //            column: x => x.TechnologiesId,
            //            principalTable: "Technologies",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            migrationBuilder.CreateTable(
                name: "JobOfferUser",
                columns: table => new
                {
                    FollowingJobOffersId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FollowingUsersId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobOfferUser", x => new { x.FollowingJobOffersId, x.FollowingUsersId });
                    table.ForeignKey(
                        name: "FK_JobOfferUser_JobOffers_FollowingJobOffersId",
                        column: x => x.FollowingJobOffersId,
                        principalTable: "JobOffers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JobOfferUser_Users_FollowingUsersId",
                        column: x => x.FollowingUsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JobOfferTechnology_TechnologiesId",
                table: "JobOfferTechnology",
                column: "TechnologiesId");

            migrationBuilder.CreateIndex(
                name: "IX_JobOfferUser_FollowingUsersId",
                table: "JobOfferUser",
                column: "FollowingUsersId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JobOfferTechnology");

            migrationBuilder.DropTable(
                name: "JobOfferUser");

            migrationBuilder.DropTable(
                name: "Technologies");

            migrationBuilder.DropTable(
                name: "JobOffers");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
