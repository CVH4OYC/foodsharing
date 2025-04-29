using System.ComponentModel.DataAnnotations;

namespace Foodsharing.API.Models;

/// <summary>
/// Избранная категория еды
/// </summary>
public class FavoriteCategory
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
    /// Id категории
    /// </summary>
    [Required]
    public Guid CategoryId { get; set; }

    /// <summary>
    /// Навигационное свойство для связи с Category
    /// </summary>
    public Category Category { get; set; } = null!;
}
