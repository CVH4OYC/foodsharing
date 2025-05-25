using Foodsharing.API.Data;
using Foodsharing.API.Interfaces.Repositories;
using Foodsharing.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Foodsharing.API.Repository;

public class CategoryRepository : Repository<Category>, ICategoryRepository
{
    private readonly DbContext context;

    public CategoryRepository(AppDbContext context) : base(context)
    {
        this.context = context;
    }

    public async Task<IEnumerable<Category>> GetAllCategoriesAsync(CancellationToken cancellationToken = default)
    {
        return await context.Set<Category>().ToListAsync(cancellationToken);
    }
}
