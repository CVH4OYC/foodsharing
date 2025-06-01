using Foodsharing.API.DTOs;
using Foodsharing.API.Models;

namespace Foodsharing.API.Interfaces.Repositories;

public interface IFavoritesRepository
{
    Task AddFavoriteCategoryAsync(FavoriteCategory category, CancellationToken cancellationToken);

    Task AddFavoriteOrganizationAsync(FavoriteOrganization organization, CancellationToken cancellationToken);

    Task<IEnumerable<FavoriteCategory>> GetFavoriteCategoriesAsync(Guid userId, CancellationToken cancellationToken);

    Task<IEnumerable<FavoriteOrganization>> GetFavoriteOrganizationsAsync(Guid userId, CancellationToken cancellationToken);

    Task DeleteFavoriteCategoryAsync(FavoriteCategory category, CancellationToken cancellationToken);

    Task DeleteFavoriteOrganizationAsync(FavoriteOrganization organization, CancellationToken cancellationToken);

    Task<FavoriteCategory?> GetFavoriteCategoryByIdAndUserAsync(Guid categoryId, Guid userId, CancellationToken cancellationToken);

    Task<FavoriteOrganization?> GetFavoriteOrganizationByIdAndUserAsync(Guid orgId, Guid userId, CancellationToken cancellationToken);

    Task<List<FavoriteCategory>> GetFavoriteCategoriesWithUsersAsync(Guid categoryId, CancellationToken cancellationToken = default);
}
