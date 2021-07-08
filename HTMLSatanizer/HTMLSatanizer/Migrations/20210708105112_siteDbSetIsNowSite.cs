using Microsoft.EntityFrameworkCore.Migrations;

namespace HTMLSatanizer.Migrations
{
    public partial class siteDbSetIsNowSite : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_siteDbSet",
                table: "siteDbSet");

            migrationBuilder.RenameTable(
                name: "siteDbSet",
                newName: "Site");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Site",
                table: "Site",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Site",
                table: "Site");

            migrationBuilder.RenameTable(
                name: "Site",
                newName: "siteDbSet");

            migrationBuilder.AddPrimaryKey(
                name: "PK_siteDbSet",
                table: "siteDbSet",
                column: "Id");
        }
    }
}
