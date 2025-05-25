using Foodsharing.API.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Foodsharing.API.Data.ModelsConficurations;

public class FavoriteCategoryConfiguration : IEntityTypeConfiguration<FavoriteCategory>
{
    public void Configure(EntityTypeBuilder<FavoriteCategory> builder)
    {
        builder.HasKey(fc => fc.Id);
        builder.HasIndex(fc => new { fc.UserId, fc.CategoryId }).IsUnique();
    }
}
