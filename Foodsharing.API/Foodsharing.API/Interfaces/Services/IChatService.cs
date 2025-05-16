using Foodsharing.API.DTOs.ChatAndMessage;

namespace Foodsharing.API.Interfaces.Services;

public interface IChatService
{
    /// <summary>
    /// Получить id чата
    /// </summary>
    /// <param name="otherUserId">id другого пользователя</param>
    /// <returns></returns>
    Task<Guid?> GetChatIdWithUserAsync(Guid otherUserId, CancellationToken cancellationToken);

    /// <summary>
    /// Создать чат с пользователем
    /// </summary>
    /// <param name="otherUserId">id другого пользователя</param>
    /// <returns></returns>
    Task<Guid> CreateChatWithUserAsync(Guid otherUserId, CancellationToken cancellationToken);

    /// <summary>
    /// Получить свои чаты
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<ChatDTO>?> GetMyChatsAsync(CancellationToken cancellationToken);

    Task<ChatWithMessagesDTO?> GetChatWithMessagesAsync(
        Guid chatId,
        int page,
        int pageSize,
        string? search,
        CancellationToken cancellationToken = default);
}
