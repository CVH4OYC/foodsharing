using Foodsharing.API.DTOs;
using Foodsharing.API.DTOs.Announcement;
using Foodsharing.API.Extensions;
using Foodsharing.API.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Foodsharing.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IFavoritesService _favoritesService;

    public CategoryController(ICategoryService categoryService, IHttpContextAccessor httpContextAccessor, IFavoritesService favoritesService)
    {
        _categoryService = categoryService;
        _httpContextAccessor = httpContextAccessor;
        _favoritesService = favoritesService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetCategoriesAsync(CancellationToken cancellationToken)
    {
        var categories = await _categoryService.GetCategoriesAsync(cancellationToken);
        return Ok(categories ?? new List<CategoryDTO>());
    }

    [HttpGet("{categoryId}")]
    public async Task<ActionResult<AnnouncementDTO>> GetCategoryByIdAsync(Guid categoryId, CancellationToken cancellationToken)
    {
        var announcement = await _categoryService.GetCategoryByIdAsync(categoryId, cancellationToken);

        return announcement == null
            ? NotFound($"Категория с ID {categoryId} не найдена")
            : Ok(announcement);
    }

    [HttpPost("favorite")]
    [Authorize]
    public async Task<IActionResult> AddFavoriteCategoryAsync(Guid categoryId, CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.HttpContext?.User.GetUserId();
        if (userId == null)
            return Unauthorized();

        await _favoritesService.AddFavoriteCategoryAsync(categoryId, (Guid)userId, cancellationToken);
        return Ok();
    }
}
