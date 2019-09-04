using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PowerBankAdmin.Migrations
{
    public partial class Holders : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Holders",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(nullable: true),
                    LocalCode = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Holders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PowerbankModel",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<int>(nullable: false),
                    PowerbanksId = table.Column<int>(nullable: true),
                    CostumerModelId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PowerbankModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PowerbankModel_Costumers_CostumerModelId",
                        column: x => x.CostumerModelId,
                        principalTable: "Costumers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PowerbankModel_Holders_PowerbanksId",
                        column: x => x.PowerbanksId,
                        principalTable: "Holders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PowerbankModel_CostumerModelId",
                table: "PowerbankModel",
                column: "CostumerModelId");

            migrationBuilder.CreateIndex(
                name: "IX_PowerbankModel_PowerbanksId",
                table: "PowerbankModel",
                column: "PowerbanksId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PowerbankModel");

            migrationBuilder.DropTable(
                name: "Holders");
        }
    }
}
