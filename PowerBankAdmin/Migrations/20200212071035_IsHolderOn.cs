using Microsoft.EntityFrameworkCore.Migrations;

namespace PowerBankAdmin.Migrations
{
    public partial class IsHolderOn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RentModels",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DropColumn(
                name: "FirstHourFree",
                table: "RentModels");

            migrationBuilder.AddColumn<bool>(
                name: "IsOn",
                table: "Holders",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "RentModels",
                keyColumn: "Id",
                keyValue: 2,
                column: "RentStrategy",
                value: 1);

            migrationBuilder.UpdateData(
                table: "RentModels",
                keyColumn: "Id",
                keyValue: 3,
                column: "RentStrategy",
                value: 2);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsOn",
                table: "Holders");

            migrationBuilder.AddColumn<bool>(
                name: "FirstHourFree",
                table: "RentModels",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "RentModels",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FirstHourFree", "RentStrategy" },
                values: new object[] { true, 0 });

            migrationBuilder.UpdateData(
                table: "RentModels",
                keyColumn: "Id",
                keyValue: 3,
                column: "RentStrategy",
                value: 1);

            migrationBuilder.InsertData(
                table: "RentModels",
                columns: new[] { "Id", "FirstHourFree", "RentStrategy" },
                values: new object[] { 4, true, 1 });
        }
    }
}
