using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HTMLSatanizer.Migrations
{
    public partial class updatingSiteModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "RecentUpdate",
                table: "Site",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RecentUpdate",
                table: "Site");
        }
    }
}
