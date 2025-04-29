using System.ComponentModel.DataAnnotations;

namespace Foodsharing.API.Models;

/// <summary>
/// Категория еды в объявлении
/// </summary>
public class Category
{
    /// <summary>
    /// Id категории продуктов питания
    /// </summary>
    [Key]
    public Guid Id { get; set; }
    
    /// <summary>
    /// Название категории продуктов питания
    /// </summary>
    [Required]
    public string Name { get; set; }
    
    /// <summary>
    /// Id родительской категории
    /// </summary>
    public Guid? ParentId { get; set; }

    /// <summary>
    /// Навигационное свойство для связи с таблицей Announcement
    /// </summary>
    public List<Announcement>? Announcements { get; set; }
}
