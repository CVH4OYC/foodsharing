using Foodsharing.API.Constants;
using Foodsharing.API.DTOs;
using Foodsharing.API.Interfaces.Repositories;
using Foodsharing.API.Interfaces.Services;
using Foodsharing.API.Models;
using Foodsharing.API.Repository;

namespace Foodsharing.API.Services;

public class NotificationService : INotificationService
{
    private readonly INotificationRepository _notificationRepository;
    private readonly IStatusesRepository _statusesRepository;

    public NotificationService(INotificationRepository notificationRepository, IStatusesRepository statusesRepository)
    {
        _notificationRepository = notificationRepository;
        _statusesRepository = statusesRepository;
    }

    public async Task<List<NotificationDTO>> GetUserNotificationsAsync(Guid userId, string? statusCode = null, CancellationToken cancellationToken = default)
    {
        var notifications = await _notificationRepository.GetUserNotificationsAsync(userId, statusCode, cancellationToken);

        return notifications.Select(n => new NotificationDTO
        {
            Id = n.Id,
            Type = n.NotificationType.Code,
            Status = n.NotificationStatus.Name,
            Message = n.Message,
            AnnouncementId = n.AnnouncementId,
            CreatedAt = n.CreatedAt
        }).ToList();
    }

    public async Task MarkAsReadAsync(Guid userId, Guid notificationId, CancellationToken cancellationToken = default)
    {
        var notification = await _notificationRepository.GetByIdAsync(notificationId, cancellationToken);
        if (notification == null || notification.UserId != userId)
            throw new Exception("Уведомление не найдено или не принадлежит вам");

        var readStatusId = await _statusesRepository.GetNotificationStatusIdByNameAsync(NotificationStatusConsts.IsRead, cancellationToken);
        if (readStatusId == null)
            throw new Exception("Статус 'прочитано' не найден");

        notification.NotificationStatusId = readStatusId.Value;

        await _notificationRepository.UpdateAsync(notification, cancellationToken);
    }

    public async Task MarkAllAsReadAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var readStatusId = await _statusesRepository.GetNotificationStatusIdByNameAsync(NotificationStatusConsts.IsRead, cancellationToken);
        if (readStatusId == null)
            throw new Exception("Статус 'прочитано' не найден");
        

        await _notificationRepository.MarkAllAsReadAsync(userId, readStatusId.Value, cancellationToken);
    }

    public async Task CreateNotificationAsync(Guid userId, string typeCode, string message, Guid? announcementId = null, CancellationToken cancellationToken = default)
    {
        var typeId = await _notificationRepository.GetTypeIdByCodeAsync(typeCode, cancellationToken);
        if (typeId == null) return;

        var unreadStatusId = await _statusesRepository.GetNotificationStatusIdByNameAsync(NotificationStatusConsts.IsUnread, cancellationToken);
        if (unreadStatusId == null) return;

        var notification = new Notification
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            NotificationTypeId = typeId.Value,
            NotificationStatusId = unreadStatusId.Value,
            Message = message,
            AnnouncementId = announcementId,
            CreatedAt = DateTime.UtcNow
        };

        await _notificationRepository.AddAsync(notification, cancellationToken);
    }
}
