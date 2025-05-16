using Foodsharing.API.Models;

namespace Foodsharing.API.Interfaces.Repositories;

public interface IStatusesRepository
{
    Task<TransactionStatus?> GetTransactionStatusByName(string name, CancellationToken cancellationToken = default);

    Task<OrganizationStatus?> GetOrganizationStatusByName(string name, CancellationToken cancellationToken = default);

    Task<PartnershipApplicationStatus?> GetPartnershipApplicationStatusByName(string name, CancellationToken cancellationToken = default);

    Task<MessageStatus?> GetMessageStatusByNameAsync(string name, CancellationToken cancellationToken = default);
}
