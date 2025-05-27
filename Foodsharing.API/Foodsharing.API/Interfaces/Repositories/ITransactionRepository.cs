using Foodsharing.API.DTOs;
using Foodsharing.API.Models;

namespace Foodsharing.API.Interfaces.Repositories;

public interface ITransactionRepository : IRepository<Transaction>
{
    IQueryable<Transaction?> GetBookedTransactions();

    Task<List<Transaction>?> GetTransactionsByAnnouncementId(Guid announcementId, CancellationToken cancellationToken);

    Task<List<Transaction>?> GetUsersBookedTransactions(Guid userId, CancellationToken cancellationToken);

    Task<Transaction?> GetBookedTransactionByAnnouncementIdAsync(Guid announcementId, CancellationToken cancellationToken);

    /// <summary>
    /// Получить обмены пользователя, где он выступал, как отдающий еду
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<IEnumerable<Transaction>> GetUsersTransactionsAsSenderAsync(Guid userId, CancellationToken cancellationToken);

    /// <summary>
    /// Получить обмены пользователя, где он выступал получателем едыы
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<IEnumerable<Transaction>> GetUsersTransactionsAsRecipientAsync(Guid userId, CancellationToken cancellationToken);

    Task<Transaction?> GetTransactionByIdAsync(Guid transactionId, CancellationToken cancellationToken);
}
