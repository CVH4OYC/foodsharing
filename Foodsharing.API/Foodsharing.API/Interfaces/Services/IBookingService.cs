using Foodsharing.API.DTOs.Announcement;
using Foodsharing.API.Infrastructure;

namespace Foodsharing.API.Interfaces.Services;

public interface IBookingService
{
    Task<OperationResult> BookAnnouncementAsync(Guid announcementId, CancellationToken cancellationToken);

    Task<OperationResult> UnbookAnnouncementAsync(Guid announcementId, CancellationToken cancellationToken);

    Task<OperationResult>  CompleteTransactionAsync(Guid announcementId, CancellationToken cancellationToken);
}
