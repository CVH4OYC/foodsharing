using Foodsharing.API.Models;

namespace Foodsharing.API.Interfaces;

public interface ITransactionRepository : IRepository<Transaction>
{
    IQueryable<Transaction?> GetBookedTransactions();

    Task<List<Transaction>?> GetTransactionsByAnnouncementId(Guid announcementId, CancellationToken cancellationToken);

    Task<List<Transaction>?> GetUsersBookedTransactions(Guid userId, CancellationToken cancellationToken);

    Task<Transaction?> GetBookedTransactionByAnnouncementIdAsync(Guid announcementId, CancellationToken cancellationToken);
}
