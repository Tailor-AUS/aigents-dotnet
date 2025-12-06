using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Aigents.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Agents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    AvatarUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AgencyName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    AgencyLogo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LicenseNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Suburbs = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Postcodes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AcceptsOffMarket = table.Column<bool>(type: "bit", nullable: false),
                    MinPropertyValue = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    MaxPropertyValue = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    PreferredPropertyTypes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ListingsReceived = table.Column<int>(type: "int", nullable: false),
                    ListingsConverted = table.Column<int>(type: "int", nullable: false),
                    TotalCommissionEarned = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsVerified = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastActiveAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Agents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    AvatarUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Provider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    InterestedSuburb = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PreferredMode = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    HandedOffAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AssignedAgentId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastActiveAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Conversations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Mode = table.Column<int>(type: "int", nullable: false),
                    Summary = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Conversations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Conversations_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Listings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Suburb = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    State = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Postcode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Bedrooms = table.Column<int>(type: "int", nullable: true),
                    Bathrooms = table.Column<int>(type: "int", nullable: true),
                    CarSpaces = table.Column<int>(type: "int", nullable: true),
                    LandSize = table.Column<int>(type: "int", nullable: true),
                    PropertyType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Headline = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: false),
                    Features = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TargetBuyers = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EstimatedValue = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    AskingPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    PriceDisplay = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ImageUrls = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    AgreementSigned = table.Column<bool>(type: "bit", nullable: false),
                    AgreementSignedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AgreementSignature = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DistributedToAgents = table.Column<bool>(type: "bit", nullable: false),
                    DistributedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AgentsNotified = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PublishedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Listings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Listings_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ConversationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TokensUsed = table.Column<int>(type: "int", nullable: true),
                    ModelUsed = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Messages_Conversations_ConversationId",
                        column: x => x.ConversationId,
                        principalTable: "Conversations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ListingDistributions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ListingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AgentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SentAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EmailSent = table.Column<bool>(type: "bit", nullable: false),
                    SmsSent = table.Column<bool>(type: "bit", nullable: false),
                    ViewedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListingDistributions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ListingDistributions_Agents_AgentId",
                        column: x => x.AgentId,
                        principalTable: "Agents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ListingDistributions_Listings_ListingId",
                        column: x => x.ListingId,
                        principalTable: "Listings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ListingInquiries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ListingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AgentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RespondedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListingInquiries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ListingInquiries_Agents_AgentId",
                        column: x => x.AgentId,
                        principalTable: "Agents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ListingInquiries_Listings_ListingId",
                        column: x => x.ListingId,
                        principalTable: "Listings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Agents_Email",
                table: "Agents",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Agents_IsActive",
                table: "Agents",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Conversations_CreatedAt",
                table: "Conversations",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Conversations_UserId",
                table: "Conversations",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ListingDistributions_AgentId",
                table: "ListingDistributions",
                column: "AgentId");

            migrationBuilder.CreateIndex(
                name: "IX_ListingDistributions_ListingId_AgentId",
                table: "ListingDistributions",
                columns: new[] { "ListingId", "AgentId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ListingDistributions_SentAt",
                table: "ListingDistributions",
                column: "SentAt");

            migrationBuilder.CreateIndex(
                name: "IX_ListingInquiries_AgentId",
                table: "ListingInquiries",
                column: "AgentId");

            migrationBuilder.CreateIndex(
                name: "IX_ListingInquiries_CreatedAt",
                table: "ListingInquiries",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ListingInquiries_ListingId",
                table: "ListingInquiries",
                column: "ListingId");

            migrationBuilder.CreateIndex(
                name: "IX_Listings_CreatedAt",
                table: "Listings",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Listings_Postcode",
                table: "Listings",
                column: "Postcode");

            migrationBuilder.CreateIndex(
                name: "IX_Listings_Status",
                table: "Listings",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Listings_Suburb",
                table: "Listings",
                column: "Suburb");

            migrationBuilder.CreateIndex(
                name: "IX_Listings_UserId",
                table: "Listings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ConversationId",
                table: "Messages",
                column: "ConversationId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_CreatedAt",
                table: "Messages",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Provider_ProviderId",
                table: "Users",
                columns: new[] { "Provider", "ProviderId" },
                unique: true,
                filter: "[ProviderId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ListingDistributions");

            migrationBuilder.DropTable(
                name: "ListingInquiries");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "Agents");

            migrationBuilder.DropTable(
                name: "Listings");

            migrationBuilder.DropTable(
                name: "Conversations");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
