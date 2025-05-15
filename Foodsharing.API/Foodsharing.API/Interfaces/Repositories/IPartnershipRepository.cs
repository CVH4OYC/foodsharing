using Foodsharing.API.DTOs;
using Foodsharing.API.Models;
using Foodsharing.API.Services;

namespace Foodsharing.API.Interfaces.Repositories;

public interface IPartnershipRepository : IRepository<PartnershipApplication>
{
    Task<List<PartnershipApplication>> GetPartnershipApplicationsAsync(
        string? search,
        string? sortBy,
        int page,
        int limit,
        string? statusFilter,
        CancellationToken cancellationToken);

    Task<PartnershipApplication?> GetPartnershipApplicationByIdAsync(Guid applicationId, CancellationToken cancellationToken);
}
