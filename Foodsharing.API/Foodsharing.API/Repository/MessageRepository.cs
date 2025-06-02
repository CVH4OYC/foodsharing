using Foodsharing.API.Data;
using Foodsharing.API.Interfaces.Repositories;
using Foodsharing.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Foodsharing.API.Repository;

public class MessageRepository : Repository<Message>, IMessageRepository
{
    private readonly DbContext context;

    public MessageRepository(AppDbContext context) : base(context)
    {
        this.context = context;
    }

    public async Task<List<Message>> GetUnreadMessagesForChatAsync(Guid chatId, Guid readerId, Guid readStatusId, CancellationToken cancellationToken = default)
    {
        return await context.Set<Message>()
            .Where(m => m.ChatId == chatId && m.SenderId != readerId && m.StatusId != readStatusId)
            .ToListAsync(cancellationToken);
    }

    public async Task UpdateRangeAsync(IEnumerable<Message> messages, CancellationToken cancellationToken = default)
    {
        context.Set<Message>().UpdateRange(messages);
        await context.SaveChangesAsync(cancellationToken);
    }
}
