using Foodsharing.API.Abstract;

namespace Foodsharing.API.DTOs.ChatAndMessage;

public class ChatWithMessagesDTO : EntityBase
{
    /// <summary>
    /// Собеседник
    /// </summary>
    public UserDTO Interlocutor { get; set; }

    /// <summary>
    /// Сообщения
    /// </summary>
    public IEnumerable<MessageDTO>? Messages { get; set; }
}
