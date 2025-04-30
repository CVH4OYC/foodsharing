namespace Foodsharing.API.Infrastructure;

/// <summary>
/// Класс для описания свойств для генерации jwt-токена
/// </summary>
public class JwtOptions
{
    /// <summary>
    /// Секретный ключ для шифрования/расшифровки токена
    /// </summary>
    public string SecretKey { get; set; } = string.Empty;
    /// <summary>
    /// Время жизни токена в часах
    /// </summary>
    public int ExpiresHours { get; set; }

    public string Issuer { get; set; }

    public string Audience { get; set; }
}