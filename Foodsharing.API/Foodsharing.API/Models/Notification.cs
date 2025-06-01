using Foodsharing.API.Abstract;
using System.ComponentModel.DataAnnotations;

namespace Foodsharing.API.Models;

public class Notification : EntityBase
{
    [Required]
    public Guid UserId { get; set; }
    public User User { get; set; }

    [Required]
    public Guid NotificationTypeId { get; set; }
    public NotificationType NotificationType { get; set; }

    [Required]
    public Guid NotificationStatusId { get; set; }
    public NotificationStatus NotificationStatus { get; set; }

    public Guid? AnnouncementId { get; set; }
    public Announcement? Announcement { get; set; }

    [MaxLength(500)]
    public string? Message { get; set; }

    public DateTime CreatedAt { get; set; }
}

