using Foodsharing.API.DTOs.ChatAndMessage;

namespace Foodsharing.API.Interfaces.Services;

public interface IMessageService
{
    Task SendMessageAsync(Guid senderId, CreateMessageDTO dto, CancellationToken cancellationToken);

    Task MarkMessagesAsReadAsync(Guid chatId, Guid readerId);
}
