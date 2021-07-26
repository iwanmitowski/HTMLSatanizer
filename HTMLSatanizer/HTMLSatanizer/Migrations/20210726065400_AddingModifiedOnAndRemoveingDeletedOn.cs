using Microsoft.EntityFrameworkCore.Migrations;

namespace HTMLSatanizer.Migrations
{
    public partial class AddingModifiedOnAndRemoveingDeletedOn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DeletedOn",
                table: "Site",
                newName: "ModifiedOn");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ModifiedOn",
                table: "Site",
                newName: "DeletedOn");
        }
    }
}
