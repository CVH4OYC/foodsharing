using Foodsharing.API.Abstract;

namespace Foodsharing.API.Models;

/// <summary>
/// Роль пользователя
/// </summary>
public class UserRole : EntityBase
{
    public Guid RoleId { get; set; }

    public Guid UserId { get; set; }

    /// <summary>
    /// Навигационное свойство для связи с таблицей Role
    /// </summary>
    public Role Role { get; set; }

    /// <summary>
    /// Навигационное свойство для связи с таблицей User
    /// </summary>
    public User User { get; set; }
}
