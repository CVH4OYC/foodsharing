using Foodsharing.API.DTOs.Announcement;
using Foodsharing.API.Infrastructure;
using Foodsharing.API.Models;

namespace Foodsharing.API.Interfaces;

public interface IAnnouncementService
{
    Task<OperationResult> AddAsync(CreateAnnouncementRequest request, CancellationToken cancellationToken = default);

    Task<IEnumerable<AnnouncementDTO>> GetAnnouncementsAsync(CancellationToken cancellationToken = default);

    Task<AnnouncementDTO> GetAnnouncementByIdAsync(Guid announcementId, CancellationToken cancellationToken = default);
}
