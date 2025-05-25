using Foodsharing.API.DTOs;
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

    public async Task<IEnumerable<CategoryDTO>> GetFavoriteCategoriesAsync(Guid userId, CancellationToken cancellationToken)
    {
        var categories = await _favoritesRepository.GetFavoriteCategoriesAsync(userId, cancellationToken);

        return categories.Select(c => new CategoryDTO
        {
            Id = c.Id,
            Name = c.Category.Name,
            Color = c.Category.Color,
            ParentId = c.Category.ParentId
        });
    }

    public async Task<IEnumerable<OrganizationDTO>> GetFavoriteOrganizationsAsync(Guid userId, CancellationToken cancellationToken)
    {
        var orgs = await _favoritesRepository.GetFavoriteOrganizationsAsync(userId, cancellationToken);

        return orgs.Select(o => new OrganizationDTO
        {
            Id = o.Id,
            Name = o.Organization.Name,
            LogoImage = o.Organization.LogoImage,
            Address = new AddressDTO
            {
                Region = o.Organization.Address.Region,
                City = o.Organization.Address.City,
                Street = o.Organization.Address.Street,
                House = o.Organization.Address.House
            }
        });
    }
}
