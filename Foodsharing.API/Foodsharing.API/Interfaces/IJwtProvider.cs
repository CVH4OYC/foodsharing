using Foodsharing.API.Models;

namespace Foodsharing.API.Interfaces;

/// <summary>
/// Интерфейс провайдера для генерации jwt-токенов
/// </summary>
public interface IJwtProvider
{
    /// <summary>
    /// Генерирует jwt-токен для указанного пользователя.
    /// </summary>
    /// <param name="user">Пользователь, для которого генерируется токен</param>
    /// <returns>Сгенерированный jwt-токен в виде строки</returns>
    string GenerateToken(User user);
}
