using Foodsharing.API.Data;
using Foodsharing.API.Interfaces.Repositories;
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
            .AsNoTracking()
            .Include(p => p.User)
            .FirstOrDefaultAsync(p => p.UserId == userId, cancellationToken);
    }

    public async Task UpdateLocationAsync(Guid userId, double latitude, double longitude, CancellationToken cancellationToken = default)
    {
        var profile = await context.Set<Profile>().FirstOrDefaultAsync(p => p.UserId == userId, cancellationToken);

        if (profile == null)
            throw new Exception("Профиль не найден");

        profile.Latitude = latitude;
        profile.Longitude = longitude;

        await context.SaveChangesAsync(cancellationToken);
    }
}
