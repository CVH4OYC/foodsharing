using Foodsharing.API.DTOs;
using Foodsharing.API.DTOs.Announcement;
using Foodsharing.API.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Foodsharing.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;   
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
}
