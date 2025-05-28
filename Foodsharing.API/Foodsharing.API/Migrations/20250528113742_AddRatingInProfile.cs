using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Foodsharing.API.Migrations
{
    /// <inheritdoc />
    public partial class AddRatingInProfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "Rating",
                table: "Profiles",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "TotalGiven",
                table: "Profiles",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "TotalRecieved",
                table: "Profiles",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "TotalGiven",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "TotalRecieved",
                table: "Profiles");
        }
    }
}
