using Microsoft.EntityFrameworkCore.Migrations;

namespace TennisBookings.Web.Migrations
{
    public partial class configuration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ConfigurationEntries",
                columns: table => new
                {
                    Key = table.Column<string>(nullable: false),
                    Value = table.Column<string>(maxLength: 256, nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfigurationEntries", x => x.Key);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
