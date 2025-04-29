using System.ComponentModel.DataAnnotations;
using Foodsharing.API.Abstract;

namespace Foodsharing.API.Models;

/// <summary>
/// Чат
/// </summary>
public class Chat : EntityBase
{
    /// <summary>
    /// Внешний ключ, указывающий на первого собеседника в чате
    /// </summary>
    [Required]
    public Guid FirstUserId { get; set; }
    
    /// <summary>
    /// Навигационное свойство для связи с первым пользователем из таблицы User
    /// </summary>
    public User FirstUser { get; set; } = null!;
    
    /// <summary>
    /// Внешний ключ, указывающий на второго собеседника в чате
    /// </summary>
    [Required]
    public Guid SecondUserId { get; set; }
    
    /// <summary>
    /// Навигационное свойство для связи со вторым пользователем из таблицы User
    /// </summary>
    public User SecondUser { get; set; } = null!;
    
    /// <summary>
    /// Навигационное свойство для связи с таблицей Message
    /// </summary>
    public List<Message>? Messages { get; set; }
}
