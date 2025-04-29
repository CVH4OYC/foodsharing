using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Foodsharing.API.Models;

namespace Foodsharing.API.Data.ModelsConficurations;
public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    /// <summary>
    /// Конфигурирует модель Transaction, задавая связи между сущностями и правила удаления
    /// </summary>
    /// <param name="builder">builder для конфигурации сущности</param>
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.HasOne(t => t.Sender)
        .WithMany(u => u.Transactions)
        .HasForeignKey(c => c.SenderId)
        .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(t => t.Recipient)
        .WithMany()
        .HasForeignKey(c => c.RecipientId)
        .OnDelete(DeleteBehavior.Restrict);
    }
}
