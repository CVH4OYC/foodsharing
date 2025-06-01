using Foodsharing.API.Constants;
using Foodsharing.API.Data;
using Foodsharing.API.Interfaces.Repositories;
using Foodsharing.API.Interfaces.Services;
using Foodsharing.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Foodsharing.API.Repository;

public class NotificationRepository : Repository<Notification>, INotificationRepository
{
    private readonly DbContext context;

    public NotificationRepository(AppDbContext context) : base(context)
    {
        this.context = context;
    }

    public async Task<List<Notification>> GetUserNotificationsAsync(Guid userId, string? status = null, CancellationToken cancellationToken = default)
    {
        var query = context.Set<Notification>()
            .Include(n => n.NotificationType)
            .Include(n => n.NotificationStatus)
            .Where(n => n.UserId == userId)
            .AsQueryable();

        if (!string.IsNullOrEmpty(status))
        {
            query = query.Where(n => n.NotificationStatus.Name == status);
        }

        return await query
            .OrderByDescending(n => n.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<Notification?> GetNotificationByIdAsync(Guid notificationId, CancellationToken cancellationToken = default)
    {
        return await context.Set<Notification>()
            .Include(n => n.NotificationType)
            .Include(n => n.NotificationStatus)
            .FirstOrDefaultAsync(n => n.Id == notificationId, cancellationToken);
    }

    public async Task MarkAllAsReadAsync(Guid userId, Guid readStatusId, CancellationToken cancellationToken = default)
    {
        var notifications = await context.Set<Notification>()
            .Where(n => n.UserId == userId && n.NotificationStatus.Name == NotificationStatusConsts.IsUnread)
            .ToListAsync(cancellationToken);

        foreach (var n in notifications)
        {
            n.NotificationStatusId = readStatusId;
        }

        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Guid?> GetTypeIdByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        var typeId = await context.Set<NotificationType>()
            .Where(t => t.Code == code)
            .Select(t => t.Id)
            .FirstOrDefaultAsync(cancellationToken);

        return typeId == Guid.Empty ? null : typeId;
    }
}
