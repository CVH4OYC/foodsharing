using Foodsharing.API.DTOs;
using Foodsharing.API.Infrastructure;
using Foodsharing.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace Foodsharing.API.Interfaces;

public interface IPartnershipService
{
    /// <summary>
    /// Обработать заявку на партнёрство
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    Task<OperationResult> ProccessPartnershipApplicationAsync(CreatePartnershipApplicationDTO dto, CancellationToken cancellationToken);

    Task<List<PartnershipApplicationDTO>> GetPartnershipApplicationsAsync(
        string? search,
        string? sortBy,
        int page,
        int limit,
        string? statusFilter,
        CancellationToken cancellationToken);

    Task<PartnershipApplicationDTO?> GetPartnershipApplicationByIdAsync(Guid applicationId, CancellationToken cancellationToken);
}
