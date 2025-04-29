using System.ComponentModel.DataAnnotations;

namespace Foodsharing.API.Models;

/// <summary>
/// Статус сообщения
/// </summary>
public class MessageStatus
{
    /// <summary>
    /// Id статуса сообщения
    /// </summary>
    [Key]
    public Guid Id { get; set; }
    
    /// <summary>
    /// Название статуса сообщения
    /// </summary>
    [Required]
    public string Name { get; set; }
    
    /// <summary>
    /// Навигационное свойство для связи с таблицей Message
    /// </summary>
    public List<Message>? Messages { get; set; }
}
