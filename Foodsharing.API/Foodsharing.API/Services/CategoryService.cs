using Foodsharing.API.DTOs;
using Foodsharing.API.Extensions;
using Foodsharing.API.Interfaces.Repositories;
using Foodsharing.API.Interfaces.Services;

namespace Foodsharing.API.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IFavoritesRepository _favoritesRepository;
    private readonly IHttpContextAccessor _contextAccessor;

    public CategoryService(ICategoryRepository categoryRepository, IFavoritesRepository favoritesRepository, IHttpContextAccessor httpContextAccessor)
    {
        _categoryRepository = categoryRepository;
        _favoritesRepository = favoritesRepository;
        _contextAccessor = httpContextAccessor;
    }

    public async Task<IEnumerable<CategoryDTO>> GetCategoriesAsync(CancellationToken cancellationToken = default)
    {
        var userId = _contextAccessor.HttpContext.User.GetUserId();
        var categories = await _categoryRepository.GetAllAsync(cancellationToken);
        var favoriteIds = new List<Guid>();

        if (userId != null)
        {
            var favoriteCategories = await _favoritesRepository.GetFavoriteCategoriesAsync((Guid)userId, cancellationToken);
            favoriteIds = favoriteCategories.Select(fc => fc.CategoryId).ToList();
        }
        return categories.Select(c => new CategoryDTO
        {
            Id = c.Id,
            ParentId = c.ParentId,
            Name = c.Name,
            Color = c.Color,
            IsFavorite = favoriteIds.Contains(c.Id)
        });
    }

    public async Task<CategoryDTO?> GetCategoryByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var userId = _contextAccessor.HttpContext.User.GetUserId();
        var favoriteIds = new List<Guid>();
        if (userId != null)
        {
            var favoriteCategories = await _favoritesRepository.GetFavoriteCategoriesAsync((Guid)userId, cancellationToken);
            favoriteIds = favoriteCategories.Select(fc => fc.Id).ToList();
        }
        var category = await _categoryRepository.GetByIdAsync(id, cancellationToken);


        return new CategoryDTO
        {
            Id = category.Id,
            ParentId = category.ParentId,
            Name = category.Name,
            Color = category.Color,
            IsFavorite = favoriteIds.Contains(category.Id)
        };
    }
}
