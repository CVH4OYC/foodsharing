using Foodsharing.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Foodsharing.API.Data.ModelsConficurations;

public class FavoriteOrganizationConfiguration : IEntityTypeConfiguration<FavoriteOrganization>
{
    public void Configure(EntityTypeBuilder<FavoriteOrganization> builder)
    {
        builder.HasKey(o => o.Id);
        builder.HasIndex(o => new { o.UserId, o.OrganizationId }).IsUnique();
    }
}
