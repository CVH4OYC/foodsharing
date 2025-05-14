using Foodsharing.API.Data;
using Foodsharing.API.Interfaces;
using Foodsharing.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Foodsharing.API.Repository;

public class UserRepository : Repository<User>, IUserRepository
{
    private readonly DbContext context;

    public UserRepository(AppDbContext context) : base(context)
    {
        this.context = context;
    }
    public async Task<User?> GetByUserNameAsync(string userName, CancellationToken cancellationToken)
    {
        return await context.Set<User>().FirstOrDefaultAsync(u => u.UserName == userName, cancellationToken);
    }

    public async Task<List<string>> GetUserRolesAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await context.Set<UserRole>()
            .Where(ur => ur.UserId == userId)
            .Include(ur => ur.Role)
            .Select(ur => ur.Role.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task AddProfileAsync (Profile profile, CancellationToken cancellationToken)
    {
        await context.AddAsync(profile, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task AddRepresentativeAsync(RepresentativeOrganization representativeOrganization, CancellationToken cancellationToken)
    {
        await context.AddAsync(representativeOrganization, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }
}
