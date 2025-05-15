using Foodsharing.API.Models;

namespace Foodsharing.API.Interfaces.Services;

/// <summary>
/// Интерфейс провайдера для генерации jwt-токенов
/// </summary>
public interface IJwtProvider
{
    /// <summary>
    /// Генерирует jwt-токен для указанного пользователя.
    /// </summary>
    /// <param name="user">Пользователь, для которого генерируется токен</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Сгенерированный jwt-токен в виде строки</returns>
    Task<string> GenerateTokenAsync(User user, CancellationToken cancellationToken = default);
}
