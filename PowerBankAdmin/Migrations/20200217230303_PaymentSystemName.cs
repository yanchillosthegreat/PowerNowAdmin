using Microsoft.EntityFrameworkCore.Migrations;

namespace PowerBankAdmin.Migrations
{
    public partial class PaymentSystemName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PaymentSystemName",
                table: "CardBidnings",
                nullable: false,
                defaultValue: string.Empty);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentSystemName",
                table: "CardBidnings");
        }
    }
}
