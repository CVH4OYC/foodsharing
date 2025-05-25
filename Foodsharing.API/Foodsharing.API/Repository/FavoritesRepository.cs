using Foodsharing.API.Data;
using Foodsharing.API.Interfaces.Repositories;
using Foodsharing.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Foodsharing.API.Repository;

public class FavoritesRepository : IFavoritesRepository
{
    private readonly DbContext _context;
    public FavoritesRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task AddFavoriteCategoryAsync(FavoriteCategory category, CancellationToken cancellationToken)
    {
        await _context.Set<FavoriteCategory>().AddAsync(category, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task AddFavoriteOrganizationAsync(FavoriteOrganization organization, CancellationToken cancellationToken)
    {
        await _context.Set<FavoriteOrganization>().AddAsync(organization, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<FavoriteCategory>> GetFavoriteCategoriesAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await _context.Set<FavoriteCategory>()
            .Include(fc => fc.Category)
            .Where(fc => fc.UserId == userId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<FavoriteOrganization>> GetFavoriteOrganizationsAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await _context.Set<FavoriteOrganization>()
            .Include(fo => fo.Organization)
                .ThenInclude(o => o.Address)
            .Where(fo => fo.UserId == userId)
            .ToListAsync(cancellationToken);
    }
}
