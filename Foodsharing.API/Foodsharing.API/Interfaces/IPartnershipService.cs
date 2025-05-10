using Foodsharing.API.DTOs;
using Foodsharing.API.Infrastructure;

namespace Foodsharing.API.Interfaces;

public interface IPartnershipService
{
    /// <summary>
    /// Обработать заявку на партнёрство
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    Task<OperationResult> ProccessPartnershipApplicationAsync(CreatePartnershipApplicationDTO dto, CancellationToken cancellationToken);
}
