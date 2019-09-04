using Microsoft.EntityFrameworkCore.Migrations;

namespace PowerBankAdmin.Migrations
{
    public partial class holder2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PowerbankModel_Costumers_CostumerModelId",
                table: "PowerbankModel");

            migrationBuilder.DropForeignKey(
                name: "FK_PowerbankModel_Holders_PowerbanksId",
                table: "PowerbankModel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PowerbankModel",
                table: "PowerbankModel");

            migrationBuilder.RenameTable(
                name: "PowerbankModel",
                newName: "Powerbanks");

            migrationBuilder.RenameColumn(
                name: "PowerbanksId",
                table: "Powerbanks",
                newName: "HolderId");

            migrationBuilder.RenameColumn(
                name: "CostumerModelId",
                table: "Powerbanks",
                newName: "CostumerId");

            migrationBuilder.RenameIndex(
                name: "IX_PowerbankModel_PowerbanksId",
                table: "Powerbanks",
                newName: "IX_Powerbanks_HolderId");

            migrationBuilder.RenameIndex(
                name: "IX_PowerbankModel_CostumerModelId",
                table: "Powerbanks",
                newName: "IX_Powerbanks_CostumerId");

            migrationBuilder.AlterColumn<string>(
                name: "LocalCode",
                table: "Holders",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Powerbanks",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Powerbanks",
                table: "Powerbanks",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Powerbanks_Costumers_CostumerId",
                table: "Powerbanks",
                column: "CostumerId",
                principalTable: "Costumers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Powerbanks_Holders_HolderId",
                table: "Powerbanks",
                column: "HolderId",
                principalTable: "Holders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Powerbanks_Costumers_CostumerId",
                table: "Powerbanks");

            migrationBuilder.DropForeignKey(
                name: "FK_Powerbanks_Holders_HolderId",
                table: "Powerbanks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Powerbanks",
                table: "Powerbanks");

            migrationBuilder.RenameTable(
                name: "Powerbanks",
                newName: "PowerbankModel");

            migrationBuilder.RenameColumn(
                name: "HolderId",
                table: "PowerbankModel",
                newName: "PowerbanksId");

            migrationBuilder.RenameColumn(
                name: "CostumerId",
                table: "PowerbankModel",
                newName: "CostumerModelId");

            migrationBuilder.RenameIndex(
                name: "IX_Powerbanks_HolderId",
                table: "PowerbankModel",
                newName: "IX_PowerbankModel_PowerbanksId");

            migrationBuilder.RenameIndex(
                name: "IX_Powerbanks_CostumerId",
                table: "PowerbankModel",
                newName: "IX_PowerbankModel_CostumerModelId");

            migrationBuilder.AlterColumn<int>(
                name: "LocalCode",
                table: "Holders",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Code",
                table: "PowerbankModel",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PowerbankModel",
                table: "PowerbankModel",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PowerbankModel_Costumers_CostumerModelId",
                table: "PowerbankModel",
                column: "CostumerModelId",
                principalTable: "Costumers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PowerbankModel_Holders_PowerbanksId",
                table: "PowerbankModel",
                column: "PowerbanksId",
                principalTable: "Holders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
