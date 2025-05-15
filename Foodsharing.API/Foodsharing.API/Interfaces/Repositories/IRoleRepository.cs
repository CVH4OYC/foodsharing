using Foodsharing.API.Models;

namespace Foodsharing.API.Interfaces.Repositories;

public interface IRoleRepository : IRepository<Role>
{
    /// <summary>
    /// Метод для получения роли по названиюe
    /// </summary>
    /// <param name="roleName">Имя роли</param>
    /// <param name="cancellationToken">Токен отмены операции (по умолчанию None)</param>
    /// <returns>Пользователь типа <see cref="User"/></returns>
    Task<Role?> GetByRoleNameAsync(string roleName, CancellationToken cancellationToken);
}
