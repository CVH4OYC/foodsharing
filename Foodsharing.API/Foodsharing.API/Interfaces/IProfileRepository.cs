using Foodsharing.API.Models;

namespace Foodsharing.API.Interfaces;

public interface IProfileRepository : IRepository<Profile>
{
    Task<Profile?> GetProfileWithUserName(Guid userId, CancellationToken cancellationToken);
}
