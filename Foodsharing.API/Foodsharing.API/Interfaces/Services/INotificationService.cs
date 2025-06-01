using Foodsharing.API.DTOs;

namespace Foodsharing.API.Interfaces.Services;

public interface INotificationService
{
    /// <summary>
    /// Получить все уведомления пользователя (с опциональной фильтрацией по статусу)
    /// </summary>
    Task<List<NotificationDTO>> GetUserNotificationsAsync(Guid userId, string? status = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Пометить одно уведомление как прочитанное
    /// </summary>
    Task MarkAsReadAsync(Guid userId, Guid notificationId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Пометить все уведомления пользователя как прочитанные
    /// </summary>
    Task MarkAllAsReadAsync(Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Создать уведомление по коду типа и статусу по умолчанию (Unread)
    /// </summary>
    Task CreateNotificationAsync(Guid userId, string typeCode, string message, Guid? announcementId = null, CancellationToken cancellationToken = default);
}
