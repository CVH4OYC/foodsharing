using Foodsharing.API.Data;
using Foodsharing.API.Interfaces.Repositories;
using Foodsharing.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Foodsharing.API.Repository;

public class ChatRepository : Repository<Chat>, IChatRepository
{
    private readonly DbContext context;

    public ChatRepository(AppDbContext context) : base(context)
    {
        this.context = context;
    }

    public async Task<Chat?> GetChatByInterlocutorsIdsAsync(Guid userId1, Guid userId2, CancellationToken cancellationToken)
    {
        return await context.Set<Chat>().FirstOrDefaultAsync(c =>
            (c.FirstUserId == userId1 && c.SecondUserId == userId2) ||
            (c.FirstUserId == userId2 && c.SecondUserId == userId1),
            cancellationToken);
    }

    public async Task<IEnumerable<Chat>?> GetMyChatsAsync(Guid currentUserId, CancellationToken cancellationToken)
    {
        return await context.Set<Chat>()
            .Include(c => c.SecondUser)
                .ThenInclude(u => u.Profile)
            .Include(c => c.FirstUser)
                .ThenInclude(u => u.Profile)
            .Include(c => c.Messages.OrderByDescending(m => m.Date).Take(1))
                .ThenInclude(m => m.Status)
            .Where(c => c.FirstUserId == currentUserId ||  c.SecondUserId == currentUserId)
            .ToListAsync(cancellationToken);
    }
}
