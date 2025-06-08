using Foodsharing.API.Abstract;

namespace Foodsharing.API.DTOs.ChatAndMessage;

public class ChatDTO : EntityBase
{
    /// <summary>
    /// Собеседник
    /// </summary>
    public UserDTO Interlocutor { get; set; }

    /// <summary>
    /// Последнее сообщение в чате
    /// </summary>
    public MessageDTO? Message { get; set; }

    /// <summary>
    /// Количество непрочитанных сообщений
    /// </summary>
    public int UnreadCount { get; set; }    
}
