using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RoadMD.EntityFrameworkCore.Migrations
{
    public partial class UpdateVehicleEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LetterCode",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "NumberCode",
                table: "Vehicles");

            migrationBuilder.AddColumn<string>(
                name: "Number",
                table: "Vehicles",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_Number",
                table: "Vehicles",
                column: "Number",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Vehicles_Number",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "Number",
                table: "Vehicles");

            migrationBuilder.AddColumn<string>(
                name: "LetterCode",
                table: "Vehicles",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NumberCode",
                table: "Vehicles",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: false,
                defaultValue: "");
        }
    }
}
