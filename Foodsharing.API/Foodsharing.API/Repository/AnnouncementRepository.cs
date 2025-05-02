using Foodsharing.API.Data;
using Foodsharing.API.Interfaces;
using Foodsharing.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Foodsharing.API.Repository;

public class AnnouncementRepository : Repository<Announcement>, IAnnouncementRepository
{
    private readonly DbContext context;

    public AnnouncementRepository(AppDbContext context) : base(context)
    {
        this.context = context;
    }

    public async Task<List<Announcement>?> GetUsersAnnouncementsAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await context.Set<Announcement>().Where(a => a.UserId == userId).ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Announcement>?> GetAllAnnouncementsAsync(CancellationToken cancellationToken)
    {
        return await context.Set<Announcement>()
            .Include(a => a.User)
            .Include(a => a.Category)
            .Include(a => a.Address)
            .Include(a => a.Transactions)
                .ThenInclude(t => t.Status)
            .ToListAsync(cancellationToken);
    }
    
    public async Task<Announcement?> GetAnnouncementByIdAsync (Guid id, CancellationToken cancellationToken)
    {
        return await context.Set<Announcement>()
            .Include(a => a.User)
            .Include(a => a.Category)
            .Include(a => a.Address)
            .Include(a => a.Transactions)
                .ThenInclude(t => t.Status)
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
    }
}
