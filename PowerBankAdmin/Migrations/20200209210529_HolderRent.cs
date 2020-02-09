using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PowerBankAdmin.Migrations
{
    public partial class HolderRent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RentModels_Holders_HolderModelId",
                table: "RentModels");

            migrationBuilder.RenameColumn(
                name: "HolderModelId",
                table: "RentModels",
                newName: "HolderRentModelId");

            migrationBuilder.RenameIndex(
                name: "IX_RentModels_HolderModelId",
                table: "RentModels",
                newName: "IX_RentModels_HolderRentModelId");

            migrationBuilder.AddColumn<int>(
                name: "HolderRentModelId",
                table: "Holders",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "HolderRents",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HolderRents", x => x.Id);
                });

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Holders_HolderRents_HolderRentModelId",
                table: "Holders");

            migrationBuilder.DropForeignKey(
                name: "FK_RentModels_HolderRents_HolderRentModelId",
                table: "RentModels");

            migrationBuilder.DropTable(
                name: "HolderRents");

            migrationBuilder.DropIndex(
                name: "IX_Holders_HolderRentModelId",
                table: "Holders");

            migrationBuilder.DropColumn(
                name: "HolderRentModelId",
                table: "Holders");

            migrationBuilder.RenameColumn(
                name: "HolderRentModelId",
                table: "RentModels",
                newName: "HolderModelId");

            migrationBuilder.RenameIndex(
                name: "IX_RentModels_HolderRentModelId",
                table: "RentModels",
                newName: "IX_RentModels_HolderModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_RentModels_Holders_HolderModelId",
                table: "RentModels",
                column: "HolderModelId",
                principalTable: "Holders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
