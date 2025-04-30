namespace Foodsharing.API.Interfaces;

public interface IPasswordHasher
{

    /// <summary>
    /// Метод, генерирующий хеш пароля
    /// </summary>
    /// <param name="password">Пароль, который нужно захешировать</param>
    /// <returns>Хеш переданного пароля в виде строки</returns>
    string Generate(string password);
    
    /// <summary>
    /// Метод, сравнивающий введённый пароль и хеш
    /// </summary>
    /// <param name="password">Пароль введённый пользователем</param>
    /// <param name="passwordHash">Хеш пароля этого пользователя</param>
    /// <returns>true, если пароль совпадает, иначе false </returns>
    bool Verify(string password, string passwordHash);
}
