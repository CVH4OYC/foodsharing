using System.ComponentModel.DataAnnotations;

namespace Foodsharing.API.DTOs.ChatAndMessage;

public class MessageDTO
{
    /// <summary>
    /// Моё или нет сообщение
    /// </summary>
    public bool IsMy { get; set; }

    public UserDTO Sender { get; set; }

    /// <summary>
    /// Текст сообщения
    /// </summary>
    [StringLength(500, ErrorMessage = "Длина сообщения превышает 500 символов!")]
    public string Text { get; set; }

    /// <summary>
    /// Дата отправки сообщения
    /// </summary>
    public DateTime Date { get; set; }

    /// <summary>
    /// Навигационное свойство для связи с таблицей MessageStatus
    /// </summary>
    public string Status { get; set; }

    /// <summary>
    /// Путь до изображения, прикреплённого к сообщению
    /// </summary>
    public string? Image { get; set; }

    /// <summary>
    /// Путь до файла, прикреплённого к сообщению
    /// </summary>
    public string? File { get; set; }
}
