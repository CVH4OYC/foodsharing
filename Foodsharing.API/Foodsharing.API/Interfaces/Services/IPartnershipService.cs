﻿using Foodsharing.API.DTOs;
using Foodsharing.API.DTOs.Parthner;
using Foodsharing.API.Infrastructure;

namespace Foodsharing.API.Interfaces.Services;

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

    Task<OperationResult> AcceptApplicationAsync(AcceptApplicationRequest request, CancellationToken cancellationToken);

    Task<OperationResult> RejectApplicationAsync(AcceptApplicationRequest request, CancellationToken cancellationToken);
}
