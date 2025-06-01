using Foodsharing.API.Abstract;

namespace Foodsharing.API.DTOs;

public class NotificationDTO : EntityBase
{
    public string Type { get; set; } = null!;

    public string Status { get; set; } = null!;

    public string? Message { get; set; }

    public Guid? AnnouncementId { get; set; }

    public DateTime CreatedAt { get; set; }
}
