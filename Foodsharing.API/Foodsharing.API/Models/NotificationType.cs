using System.ComponentModel.DataAnnotations;
using Foodsharing.API.Abstract;

namespace Foodsharing.API.Models;

public class NotificationType : EntityBase
{
    [Required]
    [MaxLength(100)]
    public string Code { get; set; } = null!;

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = null!;

    public List<Notification> Notifications { get; set; } = new();
}
