using Foodsharing.API.DTOs;

namespace Foodsharing.API.Interfaces.Services;

public interface IChatService
{
    /// <summary>
    /// Получить id чата, если его нет, то создать новый
    /// </summary>
    /// <param name="otherUserId">id другого пользователя</param>
    /// <returns></returns>
    Task<Guid> GetOrCreateChatWithUserAsync(Guid otherUserId, CancellationToken cancellationToken);

    /// <summary>
    /// Получить свои чаты
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<ChatDTO>?> GetMyChatsAsync(CancellationToken cancellationToken);
}
