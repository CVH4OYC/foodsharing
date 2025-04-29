using System.ComponentModel.DataAnnotations;
using Foodsharing.API.Abstract;

namespace Foodsharing.API.Models;

/// <summary>
/// Категория еды в объявлении
/// </summary>
public class Category : EntityBase
{
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
    /// Код цвета для отображения его на фронте
    /// </summary>
    public string? Color { get; set; }

    /// <summary>
    /// Навигационное свойство для связи с таблицей Announcement
    /// </summary>
    public List<Announcement>? Announcements { get; set; }
}
