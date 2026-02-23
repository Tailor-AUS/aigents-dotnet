using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Aigents.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class ModelUpdate_SellerExperience : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ExclusiveAgentId",
                table: "Listings",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Mode",
                table: "Listings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "AgentProposal",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ListingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AgentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CommissionRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CommissionFlat = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CommissionNotes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MarketingPlan = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SellingPoints = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProposedCampaignDays = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    RejectionReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RespondedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgentProposal", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AgentProposal_Agents_AgentId",
                        column: x => x.AgentId,
                        principalTable: "Agents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AgentProposal_Listings_ListingId",
                        column: x => x.ListingId,
                        principalTable: "Listings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BuyerOffer",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ListingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BuyerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SubmittedByAgentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BuyerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BuyerEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BuyerPhone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OfferAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SettlementDays = table.Column<int>(type: "int", nullable: false),
                    Conditions = table.Column<int>(type: "int", nullable: false),
                    ConditionNotes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OfferExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ResponseNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RespondedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BuyerOffer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BuyerOffer_Agents_SubmittedByAgentId",
                        column: x => x.SubmittedByAgentId,
                        principalTable: "Agents",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BuyerOffer_Listings_ListingId",
                        column: x => x.ListingId,
                        principalTable: "Listings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BuyerOffer_Users_BuyerId",
                        column: x => x.BuyerId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SyndicationStatus",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ListingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Platform = table.Column<int>(type: "int", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false),
                    ExternalListingId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExternalUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ErrorMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PublishedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastSyncAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SyndicationStatus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SyndicationStatus_Listings_ListingId",
                        column: x => x.ListingId,
                        principalTable: "Listings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Listings_ExclusiveAgentId",
                table: "Listings",
                column: "ExclusiveAgentId");

            migrationBuilder.CreateIndex(
                name: "IX_AgentProposal_AgentId",
                table: "AgentProposal",
                column: "AgentId");

            migrationBuilder.CreateIndex(
                name: "IX_AgentProposal_ListingId",
                table: "AgentProposal",
                column: "ListingId");

            migrationBuilder.CreateIndex(
                name: "IX_BuyerOffer_BuyerId",
                table: "BuyerOffer",
                column: "BuyerId");

            migrationBuilder.CreateIndex(
                name: "IX_BuyerOffer_ListingId",
                table: "BuyerOffer",
                column: "ListingId");

            migrationBuilder.CreateIndex(
                name: "IX_BuyerOffer_SubmittedByAgentId",
                table: "BuyerOffer",
                column: "SubmittedByAgentId");

            migrationBuilder.CreateIndex(
                name: "IX_SyndicationStatus_ListingId",
                table: "SyndicationStatus",
                column: "ListingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Listings_Agents_ExclusiveAgentId",
                table: "Listings",
                column: "ExclusiveAgentId",
                principalTable: "Agents",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Listings_Agents_ExclusiveAgentId",
                table: "Listings");

            migrationBuilder.DropTable(
                name: "AgentProposal");

            migrationBuilder.DropTable(
                name: "BuyerOffer");

            migrationBuilder.DropTable(
                name: "SyndicationStatus");

            migrationBuilder.DropIndex(
                name: "IX_Listings_ExclusiveAgentId",
                table: "Listings");

            migrationBuilder.DropColumn(
                name: "ExclusiveAgentId",
                table: "Listings");

            migrationBuilder.DropColumn(
                name: "Mode",
                table: "Listings");
        }
    }
}
