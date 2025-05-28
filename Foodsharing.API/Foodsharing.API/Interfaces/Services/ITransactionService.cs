using Foodsharing.API.DTOs;
using Foodsharing.API.Models;

namespace Foodsharing.API.Interfaces.Services;

public interface ITransactionService
{
    Task<IEnumerable<TransactionDTO>> GetUsersTransactionsAsync(Guid userId, CancellationToken cancellationToken, bool isSender);

    Task<TransactionDTO?> GetTransactionByIdAsync(Guid currentUserId, Guid transactionId, CancellationToken cancellationToken);

    /// <summary>
    /// Оценить обмен
    /// </summary>
    /// <param name="currentUserId"></param>
    /// <param name="transactionId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task RateTransactionByIdAsync(Guid currentUserId, SetRatingDTO ratingDTO, CancellationToken cancellationToken);
}
