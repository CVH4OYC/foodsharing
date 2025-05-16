using Foodsharing.API.Models;

namespace Foodsharing.API.Interfaces.Repositories;

public interface IChatRepository : IRepository<Chat>
{
    /// <summary>
    /// Получить чат по id собеседника
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<Chat?> GetChatByInterlocutorsIdsAsync(Guid userId1, Guid userId2, CancellationToken cancellationToken);

    Task<IEnumerable<Chat>?> GetMyChatsAsync(Guid currentUserId, CancellationToken cancellationToken);

    Task<Chat?> GetChatWithMessagesAsync(
        Guid chatId,
        int page,
        int pageSize,
        string? search,
        CancellationToken cancellationToken);
}
