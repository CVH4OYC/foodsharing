using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Foodsharing.API.Models;

namespace Foodsharing.API.Data.ModelsConficurations;

public class RatingConfiguration : IEntityTypeConfiguration<Rating>
{
    /// <summary>
    /// Конфигурирует модель Rating, задавая связи между сущностями и правила удаления
    /// </summary>
    /// <param name="builder">builder для конфигурации сущности</param>
    public void Configure(EntityTypeBuilder<Rating> builder)
    {
        builder.HasOne(r => r.Rater)
        .WithMany(u => u.Ratings)
        .HasForeignKey(c => c.RaterId)
        .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(r => r.RatedUser)
        .WithMany()
        .HasForeignKey(c => c.RatedUserId)
        .OnDelete(DeleteBehavior.Restrict);
    }
}