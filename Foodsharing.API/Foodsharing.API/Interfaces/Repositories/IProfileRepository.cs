using Foodsharing.API.Models;

namespace Foodsharing.API.Interfaces.Repositories;

public interface IProfileRepository : IRepository<Profile>
{
    Task<Profile?> GetProfileWithUserName(Guid userId, CancellationToken cancellationToken);
}
