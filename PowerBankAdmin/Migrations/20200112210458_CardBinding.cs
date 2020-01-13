using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PowerBankAdmin.Migrations
{
    public partial class CardBinding : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "BindId",
                table: "Costumers",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CardsStatus",
                table: "Costumers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "OrderId",
                table: "Costumers",
                nullable: true);

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

            migrationBuilder.CreateIndex(
                name: "IX_CardBindingModel_CostumerModelId",
                table: "CardBindingModel",
                column: "CostumerModelId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CardBindingModel");

            migrationBuilder.DropColumn(
                name: "CardsStatus",
                table: "Costumers");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "Costumers");

            migrationBuilder.AlterColumn<string>(
                name: "BindId",
                table: "Costumers",
                nullable: true,
                oldClrType: typeof(int));
        }
    }
}
