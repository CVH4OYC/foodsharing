using System.Threading.Tasks;
using Foodsharing.API.Models;

namespace Foodsharing.API.Interfaces;

public interface IAnnouncementRepository : IRepository<Announcement>
{
    /// <summary>
    /// Получить объявления пользователя
    /// </summary>
    /// <param name="userId">Id пользователя</param>
    /// <param name="cancellationToken">Токен отмены операции (по умолчанию None)</param>
    Task<List<Announcement>> GetUsersAnnouncementsAsync(Guid userId, string? statusFilter, CancellationToken cancellationToken);

    IQueryable<Announcement?> GetAllAnnouncements();

    Task<Announcement?> GetAnnouncementByIdAsync(Guid id, CancellationToken cancellationToken);
}
