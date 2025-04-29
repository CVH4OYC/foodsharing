using System.ComponentModel.DataAnnotations;
using Foodsharing.API.Abstract;

namespace Foodsharing.API.Models;

/// <summary>
/// Представитель организации
/// </summary>
public class RepresentativeOrganization : EntityBase
{
    /// <summary>
    /// Внешний ключ, указывающий на пользователя, который является представителем организации
    /// </summary>
    [Required]
    public Guid UserId { get; set; }

    /// <summary>
    /// Навигационное свойство для связи с таблицей User
    /// </summary>
    public User User { get; set; } = null!;
    
    /// <summary>
    /// Внешний ключ, указывающий на организацию, в которой работает представитель
    /// </summary>
    [Required]
    public Guid OrganizationId { get; set; }
    
    /// <summary>
    /// Навигационное свойство для связи с таблицей Organization
    /// </summary>
    public Organization Organization { get; set; } = null!;
}
    