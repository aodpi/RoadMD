using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RoadMD.EntityFrameworkCore.Migrations
{
    public partial class AddedInfractionCategoriesEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "NumberCode",
                table: "Vehicles",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "LetterCode",
                table: "Vehicles",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "InfractionCategories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InfractionCategories", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "InfractionCategories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("4940a717-eca6-4a9e-9374-a34a5f46e279"), "Amplasarea ilegală pe vehicul a unui număr de înmatriculare fals" },
                    { new Guid("5a4a7870-e95c-4443-883d-d2126a5efa04"), "Conducerea unui vehicul fără număr de înmatriculare" },
                    { new Guid("796b8081-bb01-45f2-a76e-0d7a03250914"), "Oprirea în locuri interzise" },
                    { new Guid("d7d5ac06-54b1-49a3-9d1f-539e5a48ccd9"), "Staţionarea sau parcarea în locuri interzise" },
                    { new Guid("fbe7c0c8-fcda-4314-860d-d3e1f2c29cd7"), "Depăşirea vitezei de circulaţie stabilită" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_InfractionCategories_Name",
                table: "InfractionCategories",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InfractionCategories");

            migrationBuilder.AlterColumn<string>(
                name: "NumberCode",
                table: "Vehicles",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(5)",
                oldMaxLength: 5);

            migrationBuilder.AlterColumn<string>(
                name: "LetterCode",
                table: "Vehicles",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(5)",
                oldMaxLength: 5);
        }
    }
}
