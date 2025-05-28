using Foodsharing.API.Data;
using Foodsharing.API.Interfaces.Repositories;
using Foodsharing.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Foodsharing.API.Repository;

public class RatingRepository : Repository<Rating>, IRatingRepository
{
    private readonly DbContext context;

    public RatingRepository(AppDbContext context) : base(context)
    {
        this.context = context;
    }

    public async Task<Rating?> GetRatingByTransactionAndRaterAsync(Guid transactionId, Guid raterId, CancellationToken cancellationToken)
    {
        return await context.Set<Rating>()
            .Where(r => r.TransactionId == transactionId && r.RaterId == raterId)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
