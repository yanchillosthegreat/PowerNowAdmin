using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PowerBankAdmin.Migrations
{
    public partial class pbsessions1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Powerbanks_Costumers_CostumerId",
                table: "Powerbanks");

            migrationBuilder.DropIndex(
                name: "IX_Powerbanks_CostumerId",
                table: "Powerbanks");

            migrationBuilder.DropColumn(
                name: "CostumerId",
                table: "Powerbanks");

            migrationBuilder.CreateTable(
                name: "PowerbankSessions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CostumerId = table.Column<int>(nullable: true),
                    PowerbankId = table.Column<int>(nullable: true),
                    Start = table.Column<DateTime>(nullable: false),
                    Finish = table.Column<DateTime>(nullable: false)
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
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PowerbankSessions_CostumerId",
                table: "PowerbankSessions",
                column: "CostumerId");

            migrationBuilder.CreateIndex(
                name: "IX_PowerbankSessions_PowerbankId",
                table: "PowerbankSessions",
                column: "PowerbankId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PowerbankSessions");

            migrationBuilder.AddColumn<int>(
                name: "CostumerId",
                table: "Powerbanks",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Powerbanks_CostumerId",
                table: "Powerbanks",
                column: "CostumerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Powerbanks_Costumers_CostumerId",
                table: "Powerbanks",
                column: "CostumerId",
                principalTable: "Costumers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
