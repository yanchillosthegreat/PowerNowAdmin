using Microsoft.EntityFrameworkCore.Migrations;

namespace PowerBankAdmin.Migrations
{
    public partial class CascadeDelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CostumerAuthorizations_Costumers_CostumerId",
                table: "CostumerAuthorizations");

            migrationBuilder.DropForeignKey(
                name: "FK_VerificationCodes_Costumers_CostumerId",
                table: "VerificationCodes");

            migrationBuilder.AddForeignKey(
                name: "FK_CostumerAuthorizations_Costumers_CostumerId",
                table: "CostumerAuthorizations",
                column: "CostumerId",
                principalTable: "Costumers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VerificationCodes_Costumers_CostumerId",
                table: "VerificationCodes",
                column: "CostumerId",
                principalTable: "Costumers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CostumerAuthorizations_Costumers_CostumerId",
                table: "CostumerAuthorizations");

            migrationBuilder.DropForeignKey(
                name: "FK_VerificationCodes_Costumers_CostumerId",
                table: "VerificationCodes");

            migrationBuilder.AddForeignKey(
                name: "FK_CostumerAuthorizations_Costumers_CostumerId",
                table: "CostumerAuthorizations",
                column: "CostumerId",
                principalTable: "Costumers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VerificationCodes_Costumers_CostumerId",
                table: "VerificationCodes",
                column: "CostumerId",
                principalTable: "Costumers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
