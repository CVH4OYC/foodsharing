using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Foodsharing.API.Models;

/// <summary>
/// Роль пользователя
/// </summary>
public class UserRole : IdentityUserRole<Guid> 
{
    /// <summary>
    /// Id сущности о роли пользователя
    /// </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// Навигационное свойство для связи с таблицей Role
    /// </summary>
    public Role Role { get; set; }
    
    /// <summary>
    /// Навигационное свойство для связи с таблицей User
    /// </summary>
    public User User { get; set; }
}
