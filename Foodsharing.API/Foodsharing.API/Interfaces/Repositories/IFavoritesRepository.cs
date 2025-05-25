using Foodsharing.API.Models;

namespace Foodsharing.API.Interfaces.Repositories;

public interface IFavoritesRepository
{
    Task AddFavoriteCategoryAsync(FavoriteCategory category, CancellationToken cancellationToken);

    Task AddFavoriteOrganizationAsync(FavoriteOrganization organization, CancellationToken cancellationToken);
}
