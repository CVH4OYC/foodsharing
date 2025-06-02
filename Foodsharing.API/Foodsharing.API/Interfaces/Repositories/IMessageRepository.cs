using Foodsharing.API.Models;

namespace Foodsharing.API.Interfaces.Repositories;

public interface IMessageRepository : IRepository<Message>
{
    Task UpdateRangeAsync(IEnumerable<Message> messages, CancellationToken cancellationToken = default);

    Task<List<Message>> GetUnreadMessagesForChatAsync(Guid chatId, Guid readerId, Guid readStatusId, CancellationToken cancellationToken = default);
}
