using System.ComponentModel.DataAnnotations;

namespace Foodsharing.API.Models;

/// <summary>
/// Избранная организация
/// </summary>
public class FavoriteOrganization
{
    /// <summary>
    /// Id
    /// </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// Id пользователя
    /// </summary>
    [Required]
    public Guid UserId { get; set; }

    /// <summary>
    /// Навигационное свойство для свзяи с User
    /// </summary>
    public User User { get; set; } = null!;

    /// <summary>
    /// Id организации
    /// </summary>
    [Required]
    public Guid OrganizationId { get; set; }

    /// <summary>
    /// Навигационное свойство для связи с Organization
    /// </summary>
    public Organization Organization { get; set; } = null!;
}
