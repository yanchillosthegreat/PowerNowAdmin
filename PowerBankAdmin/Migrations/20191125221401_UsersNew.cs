using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PowerBankAdmin.Migrations
{
    public partial class UsersNew : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OwnerLatitude",
                table: "Holders",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OwnerLongitude",
                table: "Holders",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BindId",
                table: "Costumers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Card",
                table: "Costumers",
                nullable: true);

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

            migrationBuilder.CreateIndex(
                name: "IX_TransactionModel_CostumerModelId",
                table: "TransactionModel",
                column: "CostumerModelId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TransactionModel");

            migrationBuilder.DropColumn(
                name: "OwnerLatitude",
                table: "Holders");

            migrationBuilder.DropColumn(
                name: "OwnerLongitude",
                table: "Holders");

            migrationBuilder.DropColumn(
                name: "BindId",
                table: "Costumers");

            migrationBuilder.DropColumn(
                name: "Card",
                table: "Costumers");
        }
    }
}
