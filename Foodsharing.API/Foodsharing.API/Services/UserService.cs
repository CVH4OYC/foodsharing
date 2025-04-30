
using Foodsharing.API.Infrastructure;
using Foodsharing.API.Interfaces;
using Foodsharing.API.Models;

namespace Foodsharing.API.Services;

public class UserService : IUserService
{

    private readonly IPasswordHasher passwordHasher;
    private readonly IUserRepository userRepository;
    private readonly IRoleRepository roleRepository;
    private readonly IUserRoleRepository userRoleRepository;
    private readonly IJwtProvider jwtProvider;

    public UserService(IPasswordHasher passwordHasher, IUserRepository userRepository, IRoleRepository roleRepository, IUserRoleRepository userRoleRepository, IJwtProvider jwtProvider)
    {
        this.passwordHasher = passwordHasher;
        this.userRepository = userRepository;
        this.roleRepository = roleRepository;
        this.userRoleRepository = userRoleRepository;
        this.jwtProvider = jwtProvider;
    }
    /// <summary>
    /// Метод регистрации нового пользователя
    /// </summary>
    /// <param name="userName">Имя пользователя</param>
    /// <param name="password">Пароль</param>
    /// <param name="roleName">Имя роли</param>
    /// <param name="cancellationToken">Токен отмены операции (по умолчанию None)</param>
    /// <returns>Результат операции типа <see cref="OperationResult"/></returns>
    public async Task<OperationResult> RegisterAsync(string userName, string password, string roleName, CancellationToken cancellationToken = default)
    {
        var user = await userRepository.GetByUserNameAsync(userName, cancellationToken);
        if (user != null)
            return OperationResult.FailureResult("Пользователь с таким именем уже существует");

        var hashedPassword = passwordHasher.Generate(password);

        var role = await roleRepository.GetByRoleNameAsync(roleName, cancellationToken);
        if (role == null)
            return OperationResult.FailureResult("Роль не существует");

        var newUser = new User { UserName = userName, Password = hashedPassword };
        await userRepository.AddAsync(newUser, cancellationToken);

        var userRole = new UserRole
        {
            UserId = newUser.Id,     // Присваиваем пользователя
            RoleId = role.Id      // Присваиваем роль
        };
        await userRoleRepository.AddAsync(userRole, cancellationToken);
        var token = await jwtProvider.GenerateTokenAsync(newUser);
        return OperationResult.SuccessResult("Регистрация прошла успешно", token);
    }

    /// <summary>
    /// Метод идентификации и аутентификации пользователя по имени пользователя
    /// </summary>
    /// <param name="userName">Имя пользователя</param>
    /// <param name="password">Пароль</param>
    /// <param name="cancellationToken">Токен отмены операции (по умолчанию None)</param>
    /// <returns>Результат операции типа <see cref="OperationResult"/></returns>
    public async Task<OperationResult> LoginAsync(string userName, string password, CancellationToken cancellationToken = default)
    {
        var user = await userRepository.GetByUserNameAsync(userName, cancellationToken);
        if (user == null)
            return OperationResult.FailureResult("Пользователь с таким именем не найден");
        var result = passwordHasher.Verify(password, user.Password);
        if (!result)
            return OperationResult.FailureResult("Ошибка входа, неверный пароль");

        var token = await jwtProvider.GenerateTokenAsync(user);
        return OperationResult.SuccessResult("Вход выполнен успешно", token);
    }
}
