using Foodsharing.API.Interfaces.Repositories;
using Foodsharing.API.Interfaces.Services;
using Foodsharing.API.Models;

namespace Foodsharing.API.Services;

public class FavoritesService : IFavoritesService
{
    private readonly IFavoritesRepository _favoritesRepository;
    public FavoritesService(IFavoritesRepository favoritesRepository)
    {
       _favoritesRepository = favoritesRepository;
    }
    public async Task AddFavoriteCategoryAsync(Guid categoryId, Guid userId, CancellationToken cancellationToken)
    {
        var category = new FavoriteCategory
        {
            CategoryId = categoryId,
            UserId = userId,
        };

        await _favoritesRepository.AddFavoriteCategoryAsync(category, cancellationToken);
    }

    public async Task AddFavoriteOrganizationAsync(Guid orgId, Guid userId, CancellationToken cancellationToken)
    {
        var organization = new FavoriteOrganization
        {
            OrganizationId = orgId,
            UserId = userId
        };

        await _favoritesRepository.AddFavoriteOrganizationAsync(organization, cancellationToken);
    }
}
