using Foodsharing.API.Data;
using Foodsharing.API.Models;
using Microsoft.EntityFrameworkCore;
using Foodsharing.API.Interfaces.Repositories;

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

    public async Task<PartnershipApplicationStatus?> GetPartnershipApplicationStatusByName(string name, CancellationToken cancellationToken = default)
    {
        var status = await context.Set<PartnershipApplicationStatus>()
            .Where(s => s.Name == name)
            .FirstOrDefaultAsync(cancellationToken);

        return status;
    }

    public async Task<MessageStatus?> GetMessageStatusByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        var status = await context.Set<MessageStatus>()
            .Where(s => s.Name == name)
            .FirstOrDefaultAsync(cancellationToken);

        return status;
    }

    public async Task<Guid?> GetNotificationStatusIdByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        var status = await context.Set<NotificationStatus>()
            .Where(s => s.Name == name)
            .Select(s => s.Id)
            .FirstOrDefaultAsync(cancellationToken);

        return status == Guid.Empty ? null : status;
    }
}
