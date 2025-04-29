using System.ComponentModel.DataAnnotations;

namespace Foodsharing.API.Models;

/// <summary>
/// Сообщение
/// </summary>
public class Message
{
    /// <summary>
    /// Id сообщения
    /// </summary>
    [Key]
    public Guid Id { get; set; }
    
    /// <summary>
    /// Внешний ключ, указывающий на пользователя, который отправил сообщения
    /// </summary>
    [Required]
    public Guid SenderId { get; set; }

    /// <summary>
    /// Навигационное свойство для связи с таблицей User
    /// </summary>
    public User Sender { get; set; } = null!;
    
    /// <summary>
    /// Внешний ключ, указывающий на чат, в котором отправлено сообщение
    /// </summary>
    [Required]
    public Guid ChatId { get; set; }

    /// <summary>
    /// Навигационное свойство для связи с таблицей Chat
    /// </summary>
    public Chat Chat { get; set; } = null!;
    
    /// <summary>
    /// Текст сообщения
    /// </summary>
    [StringLength(500, ErrorMessage = "Длина сообщения превышает 500 символов!")]
    [Required]
    public string Text { get; set; }
    
    /// <summary>
    /// Дата отправки сообщения
    /// </summary>
    [Required]
    public DateTime Date { get; set; }
    
    /// <summary>
    /// Внешний ключ, указывающий на статус сообщения
    /// </summary>
    [Required]
    public Guid StatusId { get; set; }
    
    /// <summary>
    /// Навигационное свойство для связи с таблицей MessageStatus
    /// </summary>
    public MessageStatus Status { get; set; } = null!;
    
    /// <summary>
    /// Путь до изображения, прикреплённого к сообщению
    /// </summary>
    public string? Image { get; set; }
    
    /// <summary>
    /// Путь до файла, прикреплённого к сообщению
    /// </summary>
    public string? File { get; set; }
}
