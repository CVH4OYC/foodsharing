using Foodsharing.API.DTOs.ChatAndMessage;

namespace Foodsharing.API.Interfaces.Services;

public interface IMessageService
{
    Task SendMessageAsync(Guid senderId, CreateMessageDTO dto, CancellationToken cancellationToken);

    /// <summary>
    /// Пометить все непрочитанные сообщения в чате как „прочитано“ и вернуть список ID этих сообщений
    /// </summary>
    Task<List<Guid>> MarkMessagesAsReadAsync(Guid chatId, Guid readerId);
}
