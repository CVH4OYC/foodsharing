using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Foodsharing.API.Models;

/// <summary>
/// Роль пользователя
/// </summary>
public class Role : IdentityRole<Guid> 
{
    /// <summary>
    /// Id роли
    /// </summary>
    [Key]
    public Guid Id { get; set; }
    
    /// <summary>
    /// Название роли
    /// </summary>
    [Required]
    public string Name { get; set; }
    
    /// <summary>
    /// Навигационное свойство для связи с таблицей UserRole
    /// </summary>
    public List<UserRole>? UserRoles { get; set; }
}
