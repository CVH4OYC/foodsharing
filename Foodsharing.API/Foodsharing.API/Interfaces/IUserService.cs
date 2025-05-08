using Foodsharing.API.DTOs;
using Foodsharing.API.Infrastructure;

namespace Foodsharing.API.Interfaces;

public interface IUserService
{
    /// <summary>
    /// Метод регистрации нового пользователя
    /// </summary>
    /// <param name="request">DTO для регистрации</param>
    /// <param name="cancellationToken">Токен отмены операции (по умолчанию None)</param>
    /// <returns>Результат операции типа <see cref="OperationResult"/></returns>
    Task<OperationResult> RegisterAsync(RegisterDTO request, string roleName, CancellationToken cancellationToken = default);

    /// <summary>
    /// Метод идентификации и аутентификации пользователя по имени пользователя
    /// </summary>
    /// <param name="loginDTO">DTO для входа</param>
    /// <param name="cancellationToken">Токен отмены операции (по умолчанию None)</param>
    /// <returns>Результат операции типа <see cref="OperationResult"/></returns>
    Task<OperationResult> LoginAsync(string userName, string password, CancellationToken cancellationToken = default);

    Task<UserWithProfileDTO> GetMyProfileAsync(CancellationToken cancellationToken);

    Task<UserWithProfileDTO> GetOtherProfileAsync(Guid userId, CancellationToken cancellationToken);
}
