using Microsoft.EntityFrameworkCore.Migrations;

namespace RentalFinal.Migrations
{
    public partial class AddTOtalPriceColumnToRentTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "TotalPrice",
                table: "Rents",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalPrice",
                table: "Rents");
        }
    }
}
