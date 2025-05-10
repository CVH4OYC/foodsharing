using Foodsharing.API.Constants;
using System.Threading;
using Foodsharing.API.Data;
using Foodsharing.API.Interfaces;
using Foodsharing.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Foodsharing.API.Repository;

/// <summary>
/// Репозиторий для работы с различными статусами
/// </summary>
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

    public async Task<OrganizationStatus?> GetOrganizationStatusByName(string name, CancellationToken cancellationToken = default)
    {
        var status = await context.Set<OrganizationStatus>()
            .Where(s => s.Name == name)
            .FirstOrDefaultAsync(cancellationToken);

        return status;
    }

    public async Task<PartnershipApplicationStatus> GetPartnershipApplicationStatusByName(string name, CancellationToken cancellationToken = default)
    {
        var status = await context.Set<PartnershipApplicationStatus>()
            .Where(s => s.Name == name)
            .FirstOrDefaultAsync(cancellationToken);

        return status;
    }
}
