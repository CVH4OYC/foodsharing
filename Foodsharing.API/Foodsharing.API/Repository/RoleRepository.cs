using Foodsharing.API.Data;
using Foodsharing.API.Interfaces.Repositories;
using Foodsharing.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Foodsharing.API.Repository;

public class RoleRepository : Repository<Role>, IRoleRepository
{
    private readonly DbContext context;

    public RoleRepository(AppDbContext context) : base(context)
    {
        this.context = context;
    }
    public async Task<Role?> GetByRoleNameAsync(string roleName, CancellationToken cancellationToken)
    {
        return await context.Set<Role>().FirstOrDefaultAsync(r => r.Name == roleName, cancellationToken);
    }
}
