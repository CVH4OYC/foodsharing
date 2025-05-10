using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Foodsharing.API.Migrations
{
    /// <inheritdoc />
    public partial class AddImageForOrganization : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LogoImage",
                table: "Organizations",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LogoImage",
                table: "Organizations");
        }
    }
}
