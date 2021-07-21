using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HTMLSatanizer.Migrations
{
    public partial class updatingTheModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Site",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "Site",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HTMLSatanized",
                table: "Site",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Site",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Site");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "Site");

            migrationBuilder.DropColumn(
                name: "HTMLSatanized",
                table: "Site");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Site");
        }
    }
}
