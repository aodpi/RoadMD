using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RoadMD.EntityFrameworkCore.Migrations
{
    public partial class AddedInfractionReportEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InfractionReports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InfractionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReportCategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InfractionReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InfractionReports_Infractions_InfractionId",
                        column: x => x.InfractionId,
                        principalTable: "Infractions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InfractionReports_ReportCategories_ReportCategoryId",
                        column: x => x.ReportCategoryId,
                        principalTable: "ReportCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InfractionReports_InfractionId",
                table: "InfractionReports",
                column: "InfractionId");

            migrationBuilder.CreateIndex(
                name: "IX_InfractionReports_ReportCategoryId",
                table: "InfractionReports",
                column: "ReportCategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InfractionReports");
        }
    }
}
