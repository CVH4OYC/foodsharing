using Foodsharing.API.Models;

namespace Foodsharing.API.Interfaces;

public interface IUserRepository : IRepository<User>
{
    /// <summary>
    /// Метод для получения User по UserName
    /// </summary>
    /// <param name="userName">Имя пользователя</param>
    /// <param name="cancellationToken">Токен отмены операции (по умолчанию None)</param>
    /// <returns>Пользователь типа <see cref="User"/></returns>
    Task<User?> GetByUserNameAsync(string userName, CancellationToken cancellationToken);
    
    /// <summary>
    /// Получить названия ролей пользователя
    /// </summary>
    /// <param name="userId">Id пользователя</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns></returns>
    Task<List<string>> GetUserRolesAsync(Guid userId, CancellationToken cancellationToken);

    Task AddProfileAsync(Profile profile, CancellationToken cancellationToken);
}
