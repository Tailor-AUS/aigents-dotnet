using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Aigents.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddProposalsAndOffers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AgentProposals_Listings_ListingId1",
                table: "AgentProposals");

            migrationBuilder.DropForeignKey(
                name: "FK_BuyerOffers_Listings_ListingId1",
                table: "BuyerOffers");

            migrationBuilder.DropIndex(
                name: "IX_BuyerOffers_ListingId1",
                table: "BuyerOffers");

            migrationBuilder.DropIndex(
                name: "IX_AgentProposals_ListingId1",
                table: "AgentProposals");

            migrationBuilder.DropColumn(
                name: "ListingId1",
                table: "BuyerOffers");

            migrationBuilder.DropColumn(
                name: "ListingId1",
                table: "AgentProposals");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ListingId1",
                table: "BuyerOffers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ListingId1",
                table: "AgentProposals",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BuyerOffers_ListingId1",
                table: "BuyerOffers",
                column: "ListingId1");

            migrationBuilder.CreateIndex(
                name: "IX_AgentProposals_ListingId1",
                table: "AgentProposals",
                column: "ListingId1");

            migrationBuilder.AddForeignKey(
                name: "FK_AgentProposals_Listings_ListingId1",
                table: "AgentProposals",
                column: "ListingId1",
                principalTable: "Listings",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BuyerOffers_Listings_ListingId1",
                table: "BuyerOffers",
                column: "ListingId1",
                principalTable: "Listings",
                principalColumn: "Id");
        }
    }
}
