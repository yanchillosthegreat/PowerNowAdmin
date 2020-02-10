using Microsoft.EntityFrameworkCore.Migrations;

namespace PowerBankAdmin.Migrations
{
    public partial class CardBindingsaddedtoAppRepository : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CardBindingModel_Costumers_CostumerModelId",
                table: "CardBindingModel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CardBindingModel",
                table: "CardBindingModel");

            migrationBuilder.RenameTable(
                name: "CardBindingModel",
                newName: "CardBidnings");

            migrationBuilder.RenameIndex(
                name: "IX_CardBindingModel_CostumerModelId",
                table: "CardBidnings",
                newName: "IX_CardBidnings_CostumerModelId");

            migrationBuilder.AddColumn<bool>(
                name: "IsLocked",
                table: "CardBidnings",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CardBidnings",
                table: "CardBidnings",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CardBidnings_Costumers_CostumerModelId",
                table: "CardBidnings",
                column: "CostumerModelId",
                principalTable: "Costumers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CardBidnings_Costumers_CostumerModelId",
                table: "CardBidnings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CardBidnings",
                table: "CardBidnings");

            migrationBuilder.DropColumn(
                name: "IsLocked",
                table: "CardBidnings");

            migrationBuilder.RenameTable(
                name: "CardBidnings",
                newName: "CardBindingModel");

            migrationBuilder.RenameIndex(
                name: "IX_CardBidnings_CostumerModelId",
                table: "CardBindingModel",
                newName: "IX_CardBindingModel_CostumerModelId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CardBindingModel",
                table: "CardBindingModel",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CardBindingModel_Costumers_CostumerModelId",
                table: "CardBindingModel",
                column: "CostumerModelId",
                principalTable: "Costumers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
