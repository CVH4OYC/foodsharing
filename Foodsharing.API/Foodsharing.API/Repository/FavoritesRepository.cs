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
}
