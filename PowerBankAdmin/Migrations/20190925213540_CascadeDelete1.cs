using Microsoft.EntityFrameworkCore.Migrations;

namespace PowerBankAdmin.Migrations
{
    public partial class CascadeDelete1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Powerbanks_Holders_HolderId",
                table: "Powerbanks");

            migrationBuilder.DropForeignKey(
                name: "FK_PowerbankSessions_Powerbanks_PowerbankId",
                table: "PowerbankSessions");

            migrationBuilder.AddForeignKey(
                name: "FK_Powerbanks_Holders_HolderId",
                table: "Powerbanks",
                column: "HolderId",
                principalTable: "Holders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PowerbankSessions_Powerbanks_PowerbankId",
                table: "PowerbankSessions",
                column: "PowerbankId",
                principalTable: "Powerbanks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Powerbanks_Holders_HolderId",
                table: "Powerbanks");

            migrationBuilder.DropForeignKey(
                name: "FK_PowerbankSessions_Powerbanks_PowerbankId",
                table: "PowerbankSessions");

            migrationBuilder.AddForeignKey(
                name: "FK_Powerbanks_Holders_HolderId",
                table: "Powerbanks",
                column: "HolderId",
                principalTable: "Holders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PowerbankSessions_Powerbanks_PowerbankId",
                table: "PowerbankSessions",
                column: "PowerbankId",
                principalTable: "Powerbanks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
