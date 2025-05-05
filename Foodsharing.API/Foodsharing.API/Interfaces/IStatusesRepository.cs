using Foodsharing.API.Models;

namespace Foodsharing.API.Interfaces;

public interface IStatusesRepository
{
    Task<TransactionStatus?> GetTransactionStatusByName(string name, CancellationToken cancellationToken = default);
}
