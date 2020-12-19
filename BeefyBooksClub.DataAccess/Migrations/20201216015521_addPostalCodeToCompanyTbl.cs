using Microsoft.EntityFrameworkCore.Migrations;

namespace BeefyBooksClub.DataAccess.Migrations
{
    public partial class addPostalCodeToCompanyTbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PostalCode",
                table: "Companies",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PostalCode",
                table: "Companies");
        }
    }
}
