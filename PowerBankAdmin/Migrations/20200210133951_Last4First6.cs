using Microsoft.EntityFrameworkCore.Migrations;

namespace PowerBankAdmin.Migrations
{
    public partial class Last4First6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MaskedPan",
                table: "CardBindingModel",
                newName: "LastDigits");

            migrationBuilder.AddColumn<string>(
                name: "FirstDigits",
                table: "CardBindingModel",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstDigits",
                table: "CardBindingModel");

            migrationBuilder.RenameColumn(
                name: "LastDigits",
                table: "CardBindingModel",
                newName: "MaskedPan");
        }
    }
}
