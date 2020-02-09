using Microsoft.EntityFrameworkCore.Migrations;

namespace PowerBankAdmin.Migrations
{
    public partial class HolderCorrectRent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Holders_HolderRents_HolderRentModelId",
                table: "Holders");

            migrationBuilder.DropForeignKey(
                name: "FK_RentModels_HolderRents_HolderRentModelId",
                table: "RentModels");

            migrationBuilder.DropIndex(
                name: "IX_RentModels_HolderRentModelId",
                table: "RentModels");

            migrationBuilder.DropIndex(
                name: "IX_Holders_HolderRentModelId",
                table: "Holders");

            migrationBuilder.DropColumn(
                name: "HolderRentModelId",
                table: "RentModels");

            migrationBuilder.DropColumn(
                name: "HolderRentModelId",
                table: "Holders");

            migrationBuilder.AddColumn<int>(
                name: "HolderModelId",
                table: "HolderRents",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RentModelId",
                table: "HolderRents",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_HolderRents_HolderModelId",
                table: "HolderRents",
                column: "HolderModelId");

            migrationBuilder.CreateIndex(
                name: "IX_HolderRents_RentModelId",
                table: "HolderRents",
                column: "RentModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_HolderRents_Holders_HolderModelId",
                table: "HolderRents",
                column: "HolderModelId",
                principalTable: "Holders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HolderRents_RentModels_RentModelId",
                table: "HolderRents",
                column: "RentModelId",
                principalTable: "RentModels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HolderRents_Holders_HolderModelId",
                table: "HolderRents");

            migrationBuilder.DropForeignKey(
                name: "FK_HolderRents_RentModels_RentModelId",
                table: "HolderRents");

            migrationBuilder.DropIndex(
                name: "IX_HolderRents_HolderModelId",
                table: "HolderRents");

            migrationBuilder.DropIndex(
                name: "IX_HolderRents_RentModelId",
                table: "HolderRents");

            migrationBuilder.DropColumn(
                name: "HolderModelId",
                table: "HolderRents");

            migrationBuilder.DropColumn(
                name: "RentModelId",
                table: "HolderRents");

            migrationBuilder.AddColumn<int>(
                name: "HolderRentModelId",
                table: "RentModels",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HolderRentModelId",
                table: "Holders",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RentModels_HolderRentModelId",
                table: "RentModels",
                column: "HolderRentModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Holders_HolderRentModelId",
                table: "Holders",
                column: "HolderRentModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Holders_HolderRents_HolderRentModelId",
                table: "Holders",
                column: "HolderRentModelId",
                principalTable: "HolderRents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RentModels_HolderRents_HolderRentModelId",
                table: "RentModels",
                column: "HolderRentModelId",
                principalTable: "HolderRents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
