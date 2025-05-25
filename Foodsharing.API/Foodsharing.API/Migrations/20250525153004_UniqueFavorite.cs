using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Foodsharing.API.Migrations
{
    /// <inheritdoc />
    public partial class UniqueFavorite : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FavoriteOrganizations_UserId",
                table: "FavoriteOrganizations");

            migrationBuilder.DropIndex(
                name: "IX_FavoriteCategories_UserId",
                table: "FavoriteCategories");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteOrganizations_UserId_OrganizationId",
                table: "FavoriteOrganizations",
                columns: new[] { "UserId", "OrganizationId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteCategories_UserId_CategoryId",
                table: "FavoriteCategories",
                columns: new[] { "UserId", "CategoryId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FavoriteOrganizations_UserId_OrganizationId",
                table: "FavoriteOrganizations");

            migrationBuilder.DropIndex(
                name: "IX_FavoriteCategories_UserId_CategoryId",
                table: "FavoriteCategories");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteOrganizations_UserId",
                table: "FavoriteOrganizations",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteCategories_UserId",
                table: "FavoriteCategories",
                column: "UserId");
        }
    }
}
