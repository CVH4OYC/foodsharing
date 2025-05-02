using Foodsharing.API.Models;

namespace Foodsharing.API.Interfaces;

public interface IUserRoleRepository : IRepository<UserRole>
{
    //Task AddAsync(UserRole userRole, CancellationToken cancellationToken = default);
}
