using Microsoft.EntityFrameworkCore.Migrations;

namespace RentalFinal.Migrations
{
    public partial class CreateIsCompleteColumToRentTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsComplete",
                table: "Rents",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsComplete",
                table: "Rents");
        }
    }
}
