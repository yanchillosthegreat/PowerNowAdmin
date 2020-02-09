using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PowerBankAdmin.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Costumers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Phone = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Card = table.Column<string>(nullable: true),
                    BindId = table.Column<int>(nullable: false),
                    CostumerStatus = table.Column<int>(nullable: false),
                    OrderId = table.Column<string>(nullable: true),
                    CardsStatus = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Costumers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Holders",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(nullable: true),
                    LocalCode = table.Column<string>(nullable: true),
                    OwnerName = table.Column<string>(nullable: true),
                    OwnerAddress = table.Column<string>(nullable: true),
                    OwnerLatitude = table.Column<string>(nullable: true),
                    OwnerLongitude = table.Column<string>(nullable: true),
                    Schedule = table.Column<string>(nullable: true),
                    Comment = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Holders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Login = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CardBindingModel",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BindingId = table.Column<string>(nullable: true),
                    MaskedPan = table.Column<string>(nullable: true),
                    ExpiryDate = table.Column<string>(nullable: true),
                    CostumerModelId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardBindingModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CardBindingModel_Costumers_CostumerModelId",
                        column: x => x.CostumerModelId,
                        principalTable: "Costumers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CostumerAuthorizations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AuthToken = table.Column<string>(nullable: true),
                    CostumerId = table.Column<int>(nullable: true),
                    AuthType = table.Column<int>(nullable: false),
                    CreationDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CostumerAuthorizations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CostumerAuthorizations_Costumers_CostumerId",
                        column: x => x.CostumerId,
                        principalTable: "Costumers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TransactionModel",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Amount = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    CostumerModelId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransactionModel_Costumers_CostumerModelId",
                        column: x => x.CostumerModelId,
                        principalTable: "Costumers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VerificationCodes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<int>(nullable: false),
                    CostumerId = table.Column<int>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VerificationCodes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VerificationCodes_Costumers_CostumerId",
                        column: x => x.CostumerId,
                        principalTable: "Costumers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Powerbanks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(nullable: true),
                    HolderId = table.Column<int>(nullable: true),
                    Position = table.Column<int>(nullable: false),
                    Electricity = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Powerbanks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Powerbanks_Holders_HolderId",
                        column: x => x.HolderId,
                        principalTable: "Holders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RentModels",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RentStrategy = table.Column<int>(nullable: false),
                    FirstHourFree = table.Column<bool>(nullable: false),
                    HolderModelId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RentModels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RentModels_Holders_HolderModelId",
                        column: x => x.HolderModelId,
                        principalTable: "Holders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Authorizations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AuthToken = table.Column<string>(nullable: true),
                    UserId = table.Column<int>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authorizations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Authorizations_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PowerbankSessions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CostumerId = table.Column<int>(nullable: true),
                    PowerbankId = table.Column<int>(nullable: true),
                    Start = table.Column<DateTime>(nullable: false),
                    Finish = table.Column<DateTime>(nullable: false),
                    CardId = table.Column<string>(nullable: true),
                    RentModelId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PowerbankSessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PowerbankSessions_Costumers_CostumerId",
                        column: x => x.CostumerId,
                        principalTable: "Costumers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PowerbankSessions_Powerbanks_PowerbankId",
                        column: x => x.PowerbankId,
                        principalTable: "Powerbanks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PowerbankSessions_RentModels_RentModelId",
                        column: x => x.RentModelId,
                        principalTable: "RentModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "RentModels",
                columns: new[] { "Id", "FirstHourFree", "HolderModelId", "RentStrategy" },
                values: new object[,]
                {
                    { 1, false, null, 0 },
                    { 2, true, null, 0 },
                    { 3, false, null, 1 },
                    { 4, true, null, 1 }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Login", "Password" },
                values: new object[] { 1, "admin", "admin" });

            migrationBuilder.CreateIndex(
                name: "IX_Authorizations_UserId",
                table: "Authorizations",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CardBindingModel_CostumerModelId",
                table: "CardBindingModel",
                column: "CostumerModelId");

            migrationBuilder.CreateIndex(
                name: "IX_CostumerAuthorizations_CostumerId",
                table: "CostumerAuthorizations",
                column: "CostumerId");

            migrationBuilder.CreateIndex(
                name: "IX_Powerbanks_HolderId",
                table: "Powerbanks",
                column: "HolderId");

            migrationBuilder.CreateIndex(
                name: "IX_PowerbankSessions_CostumerId",
                table: "PowerbankSessions",
                column: "CostumerId");

            migrationBuilder.CreateIndex(
                name: "IX_PowerbankSessions_PowerbankId",
                table: "PowerbankSessions",
                column: "PowerbankId");

            migrationBuilder.CreateIndex(
                name: "IX_PowerbankSessions_RentModelId",
                table: "PowerbankSessions",
                column: "RentModelId");

            migrationBuilder.CreateIndex(
                name: "IX_RentModels_HolderModelId",
                table: "RentModels",
                column: "HolderModelId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionModel_CostumerModelId",
                table: "TransactionModel",
                column: "CostumerModelId");

            migrationBuilder.CreateIndex(
                name: "IX_VerificationCodes_CostumerId",
                table: "VerificationCodes",
                column: "CostumerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Authorizations");

            migrationBuilder.DropTable(
                name: "CardBindingModel");

            migrationBuilder.DropTable(
                name: "CostumerAuthorizations");

            migrationBuilder.DropTable(
                name: "PowerbankSessions");

            migrationBuilder.DropTable(
                name: "TransactionModel");

            migrationBuilder.DropTable(
                name: "VerificationCodes");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Powerbanks");

            migrationBuilder.DropTable(
                name: "RentModels");

            migrationBuilder.DropTable(
                name: "Costumers");

            migrationBuilder.DropTable(
                name: "Holders");
        }
    }
}
