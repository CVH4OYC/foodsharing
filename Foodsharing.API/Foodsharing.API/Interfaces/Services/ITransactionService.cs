using Foodsharing.API.DTOs;
using Foodsharing.API.Models;

namespace Foodsharing.API.Interfaces.Services;

public interface ITransactionService
{
    Task<IEnumerable<TransactionDTO>> GetUsersTransactionsAsync(Guid userId, CancellationToken cancellationToken, bool isSender);
}
