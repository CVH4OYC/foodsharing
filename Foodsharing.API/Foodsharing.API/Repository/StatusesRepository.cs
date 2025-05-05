using Foodsharing.API.Constants;
using System.Threading;
using Foodsharing.API.Data;
using Foodsharing.API.Interfaces;
using Foodsharing.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Foodsharing.API.Repository;

public class StatusesRepository : IStatusesRepository
{
    private readonly DbContext context;

    public StatusesRepository(AppDbContext context)
    {
        this.context = context;
    }

    public async Task<TransactionStatus?> GetTransactionStatusByName(string name, CancellationToken cancellationToken = default)
    {
        var status = await context.Set<TransactionStatus>()
            .Where(s => s.Name == name)
            .FirstOrDefaultAsync(cancellationToken);

        return status;
    }
}
