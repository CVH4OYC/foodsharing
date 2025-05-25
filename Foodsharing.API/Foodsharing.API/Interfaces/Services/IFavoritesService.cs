using Foodsharing.API.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Foodsharing.API.Interfaces.Services;

public interface IFavoritesService
{
    Task AddFavoriteCategoryAsync(Guid categoryId, Guid userId, CancellationToken cancellationToken);

    Task AddFavoriteOrganizationAsync(Guid orgId, Guid userId, CancellationToken cancellationToken);

    Task DeleteFavoriteCategoryAsync(Guid categoryId, Guid userId, CancellationToken cancellationToken);

    Task DeleteFavoriteOrganizationAsync(Guid orgId, Guid userId, CancellationToken cancellationToken);

    Task<IEnumerable<CategoryDTO>> GetFavoriteCategoriesAsync(Guid userId, CancellationToken cancellationToken);

    Task<IEnumerable<OrganizationDTO>> GetFavoriteOrganizationsAsync(Guid userId, CancellationToken cancellationToken);
}   
