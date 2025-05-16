using Foodsharing.API.DTOs.Announcement;
using Foodsharing.API.Infrastructure;

namespace Foodsharing.API.Interfaces.Services;

public interface IAnnouncementService
{
    Task<OperationResult> AddAsync(AnnouncemenstCreateUpdRequest request, CancellationToken cancellationToken = default);

    Task<IEnumerable<AnnouncementDTO>> GetAnnouncementsAsync(Guid? categoryId, string? search, string? statusFilter, string? sortBy, int page, int limit, CancellationToken cancellationToken);

    Task<AnnouncementDTO?> GetAnnouncementByIdAsync(Guid announcementId, CancellationToken cancellationToken = default);

    Task<OperationResult> UpdateAsync(Guid userId, AnnouncemenstCreateUpdRequest request, CancellationToken cancellationToken = default);

    Task<OperationResult> DeleteAnnouncementByIdAsync(Guid announcementId, CancellationToken cancellationToken = default);

    Task<IEnumerable<AnnouncementDTO>> GetMyAnnouncmentsAsync(CancellationToken cancellationToken, string? statusFilter);

    Task<IEnumerable<AnnouncementDTO>> GetOtherAnnouncmentsAsync(Guid userId, CancellationToken cancellationToken);
}
