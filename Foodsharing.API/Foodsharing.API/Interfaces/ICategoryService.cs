using Foodsharing.API.DTOs;
using Foodsharing.API.DTOs.Announcement;

namespace Foodsharing.API.Interfaces;

public interface ICategoryService
{
    Task<IEnumerable<CategoryDTO>> GetCategoriesAsync(CancellationToken cancellationToken = default);

    Task<CategoryDTO?> GetCategoryByIdAsync(Guid announcementId, CancellationToken cancellationToken = default);
}
