using Foodsharing.API.Models;

namespace Foodsharing.API.Interfaces.Repositories;

public interface IRatingRepository : IRepository<Rating>
{
    Task<Rating?> GetRatingByTransactionAndRaterAsync(Guid transactionId, Guid raterId, CancellationToken cancellationToken);
}
