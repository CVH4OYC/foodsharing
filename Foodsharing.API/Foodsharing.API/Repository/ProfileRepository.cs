using Foodsharing.API.Data;
using Foodsharing.API.Interfaces;
using Foodsharing.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Foodsharing.API.Repository;

public class ProfileRepository : Repository<Profile>, IProfileRepository
{
    private readonly DbContext context;

    public ProfileRepository(AppDbContext context) : base(context)
    {
        this.context = context;
    }

    public async Task<Profile?> GetProfileWithUserName(Guid userId, CancellationToken cancellationToken)
    {
        return await context.Set<Profile>()
            .Include(p => p.User)
            .FirstOrDefaultAsync(p => p.UserId == userId, cancellationToken);
    }
}
