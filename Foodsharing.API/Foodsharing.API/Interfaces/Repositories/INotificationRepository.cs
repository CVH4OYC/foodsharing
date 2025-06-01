using Foodsharing.API.Models;

namespace Foodsharing.API.Interfaces.Repositories;

public interface INotificationRepository : IRepository<Notification>
{
    Task<List<Notification>> GetUserNotificationsAsync(Guid userId, string? status = null, CancellationToken cancellationToken = default);

    Task MarkAllAsReadAsync(Guid userId, Guid readStatusId, CancellationToken cancellationToken = default);

    Task<Notification?> GetNotificationByIdAsync(Guid notificationId, CancellationToken cancellationToken = default);

    Task<Guid?> GetTypeIdByCodeAsync(string code, CancellationToken cancellationToken = default);
}
