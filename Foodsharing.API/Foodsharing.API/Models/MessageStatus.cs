using System.ComponentModel.DataAnnotations;
using Foodsharing.API.Abstract;

namespace Foodsharing.API.Models;

/// <summary>
/// Статус сообщения
/// </summary>
public class MessageStatus : EntityBase
{
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
