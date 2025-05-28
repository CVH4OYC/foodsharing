using Foodsharing.API.Constants;
using Foodsharing.API.Data;
using Foodsharing.API.Interfaces.Repositories;
using Foodsharing.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Foodsharing.API.Repository;

public class TransactionRepository : Repository<Transaction>, ITransactionRepository
{
    private readonly DbContext context;

    public TransactionRepository(AppDbContext context) : base(context)
    {
        this.context = context;
    }
    public IQueryable<Transaction?> GetBookedTransactions()
    {
        return context.Set<Transaction>()
            .Include(t => t.Status);
    }

    /// <summary>
    /// Получить транзакцию бронирования для объявления
    /// </summary>
    /// <param name="announcementId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<Transaction?> GetBookedTransactionByAnnouncementIdAsync(Guid announcementId, CancellationToken cancellationToken)
    {
        return await context.Set<Transaction>()
            .Include(t => t.Status)
            .FirstOrDefaultAsync(
                t => t.AnnouncementId == announcementId && t.Status.Name == TransactionStatusesConsts.IsBooked,
                cancellationToken
            );
    }
    public async Task<List<Transaction>?> GetTransactionsByAnnouncementId(Guid announcementId, CancellationToken cancellationToken)
    {
        return await context.Set<Transaction?>()
            .Include(t => t.Status)
            .Where(t => t.AnnouncementId == announcementId).ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Получить транзакции, в которых пользователь забронировал объявления
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<List<Transaction>?> GetUsersBookedTransactions(Guid userId, CancellationToken cancellationToken)
    {
        return await context.Set<Transaction>()
            .Where(t => t.RecipientId == userId && t.Status.Name == TransactionStatusesConsts.IsBooked)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Transaction>> GetUsersTransactionsAsSenderAsync(Guid userId, CancellationToken cancellationToken)
    {
        IQueryable<Transaction> query = context.Set<Transaction>()
            .Include(t => t.Sender)
                .ThenInclude(s => s.Profile)
            .Include(t => t.Sender)
                .ThenInclude(s => s.Representative)
                .ThenInclude(r => r.Organization)
            .Include(t => t.Recipient)
                .ThenInclude(r => r.Profile)
            .Include(t => t.Status)
            .Include(t => t.Announcement)
            .Where(t => t.SenderId == userId && t.Status.Name != TransactionStatusesConsts.IsCanceled)
            .OrderByDescending(t => t.TransactionDate);

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Transaction>> GetUsersTransactionsAsRecipientAsync(Guid userId, CancellationToken cancellationToken)
    {
        IQueryable<Transaction> query = context.Set<Transaction>()
            .Include(t => t.Sender)
                .ThenInclude(s => s.Profile)
            .Include(t => t.Sender)
                .ThenInclude(s => s.Representative)
                .ThenInclude(r => r.Organization)
            .Include(t => t.Recipient)
                .ThenInclude(r => r.Profile)
            .Include(t => t.Status)
            .Include(t => t.Announcement)
            .Where(t => t.RecipientId == userId && t.Status.Name != TransactionStatusesConsts.IsCanceled)
            .OrderByDescending(t => t.TransactionDate);

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<Transaction?> GetTransactionByIdAsync(Guid transactionId, CancellationToken cancellationToken)
    {
        return await context.Set<Transaction>()
            .AsNoTracking()
            .Include(t => t.Sender)
                .ThenInclude(s => s.Profile)
            .Include(t => t.Sender)
                .ThenInclude(s => s.Representative)
                .ThenInclude(r => r.Organization)
            .Include(t => t.Recipient)
                .ThenInclude(r => r.Profile)
            .Include(t => t.Recipient)
                .ThenInclude(r => r.Representative)
                .ThenInclude(r => r.Organization)
            .Include(t => t.Status)
            .Include(t => t.Announcement)
            .Where(t => t.Id == transactionId)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
