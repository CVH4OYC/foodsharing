using Foodsharing.API.Constants;
using Foodsharing.API.Data;
using Foodsharing.API.Interfaces.Repositories;
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

    public async Task<List<Announcement>> GetUsersAnnouncementsAsync(Guid userId, string? statusFilter, CancellationToken cancellationToken)
    {
        IQueryable<Announcement> query = context.Set<Announcement>()
            .Include(a => a.User).ThenInclude(u => u.Profile)
            .Include(a => a.Category)
            .Include(a => a.Address)
            .Include(a => a.Transactions).ThenInclude(t => t.Status)
            .Where(a => a.UserId == userId);

        if (!string.IsNullOrEmpty(statusFilter))
        {
            query = statusFilter switch
            {
                "active" => query.Where(a =>
                    !a.Transactions.Any() ||
                    a.Transactions
                        .OrderByDescending(t => t.TransactionDate) 
                        .Select(t => t.Status.Name)
                        .FirstOrDefault() == TransactionStatusesConsts.IsCanceled ||
                    a.Transactions
                        .OrderByDescending(t => t.TransactionDate)
                        .Select(t => t.Status.Name)
                        .FirstOrDefault() == TransactionStatusesConsts.IsBooked),

                "booked" => query.Where(a =>
                    a.Transactions
                        .OrderByDescending(t => t.TransactionDate)
                        .Select(t => t.Status.Name)
                        .FirstOrDefault() == TransactionStatusesConsts.IsBooked),

                "completed" => query.Where(a =>
                    a.Transactions
                        .OrderByDescending(t => t.TransactionDate)
                        .Select(t => t.Status.Name)
                        .FirstOrDefault() == TransactionStatusesConsts.IsCompleted),

                _ => query
            };
        }

        return await query.ToListAsync(cancellationToken);
    }

    public IQueryable<Announcement?> GetAllAnnouncements()
    {
        return context.Set<Announcement>()
            .Include(a => a.User)
                .ThenInclude(u => u.Profile)
            .Include(a => a.User)
                .ThenInclude(a => a.Representative)
                .ThenInclude(r => r.Organization)
            .Include(a => a.Category)
            .Include(a => a.Address)
            .Include(a => a.Transactions)
                .ThenInclude(t => t.Status);
    }
    
    public async Task<Announcement?> GetAnnouncementByIdAsync (Guid id, CancellationToken cancellationToken)
    {
        return await context.Set<Announcement>()
            .Include(a => a.User)
                .ThenInclude(u => u.Profile)
            .Include(a => a.User)
                .ThenInclude(a => a.Representative)
                .ThenInclude(r => r.Organization)
            .Include(a => a.Category)
            .Include(a => a.Address)
            .Include(a => a.Transactions)
                .ThenInclude(t => t.Status)
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
    }

}
