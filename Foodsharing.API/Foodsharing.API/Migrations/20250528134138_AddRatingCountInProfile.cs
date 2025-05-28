using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Foodsharing.API.Migrations
{
    /// <inheritdoc />
    public partial class AddRatingCountInProfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "RatingCount",
                table: "Profiles",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RatingCount",
                table: "Profiles");
        }
    }
}
