using Foodsharing.API.DTOs;
using Foodsharing.API.Models;

namespace Foodsharing.API.Interfaces.Repositories;

public interface IFavoritesRepository
{
    Task AddFavoriteCategoryAsync(FavoriteCategory category, CancellationToken cancellationToken);

    Task AddFavoriteOrganizationAsync(FavoriteOrganization organization, CancellationToken cancellationToken);

    Task<IEnumerable<FavoriteCategory>> GetFavoriteCategoriesAsync(Guid userId, CancellationToken cancellationToken);

    Task<IEnumerable<FavoriteOrganization>> GetFavoriteOrganizationsAsync(Guid userId, CancellationToken cancellationToken);
}
