using Foodsharing.API.Constants;
using Foodsharing.API.DTOs;
using Foodsharing.API.Infrastructure;
using Foodsharing.API.Interfaces;
using Foodsharing.API.Models;
using Microsoft.AspNetCore.Identity;

namespace Foodsharing.API.Services;

public class UserService : IUserService
{
    private readonly UserManager<User> _userManager;
    private readonly IJwtProvider _jwtProvider;
    private readonly RoleManager<Role> _roleManager;

    public UserService(UserManager<User> userManager, IJwtProvider jwtProvider, RoleManager<Role> roleManager)
    {
        _jwtProvider = jwtProvider;
        _userManager = userManager;
        _roleManager = roleManager;
        _roleManager = roleManager;
    }
    /// <summary>
    /// Метод регистрации нового пользователя
    /// </summary>
    /// <param name="userName">Имя пользователя</param>
    /// <param name="password">Пароль</param>
    /// <param name="cancellationToken">Токен отмены операции (по умолчанию None)</param>
    /// <returns>Результат операции типа <see cref="OperationResult"/></returns>
    public async Task<OperationResult> RegisterAsync(RegisterDTO regDTO, CancellationToken cancellationToken = default)
    {
        var roles = _roleManager.Roles.ToList();
        var user = await _userManager.FindByNameAsync(regDTO.UserName);
        if (user != null)
            return OperationResult.FailureResult("Пользователь с таким именем уже существует");

        var newUser = new User
        {
            UserName = regDTO.UserName,
            Email = regDTO.Email
        };

        var result = await _userManager.CreateAsync(newUser, regDTO.Password);

        if (!result.Succeeded)
            return OperationResult.FailureResult("Регистрация не удалась");

        await _userManager.AddToRoleAsync(newUser, RolesConsts.Admin);

        var token = _jwtProvider.GenerateToken(newUser);

        return OperationResult.SuccessResult("Регистрация прошла успешно", token);
    }

    /// <summary>
    /// Метод идентификации и аутентификации пользователя по имени пользователя
    /// </summary>
    /// <param name="userName">Имя пользователя</param>
    /// <param name="password">Пароль</param>
    /// <param name="cancellationToken">Токен отмены операции (по умолчанию None)</param>
    /// <returns>Результат операции типа <see cref="OperationResult"/></returns>
    public async Task<OperationResult> LoginAsync(LoginDTO loginDTO, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByNameAsync(loginDTO.UserName);
        if (user == null)
            return OperationResult.FailureResult("Пользователь с таким именем не найден");

        var passwordValid = await _userManager.CheckPasswordAsync(user, loginDTO.Password);
        if (!passwordValid)
            return OperationResult.FailureResult("Неправильное имя пользователя или пароль");

        var token = _jwtProvider.GenerateToken(user);
        return OperationResult.SuccessResult("Вход выполнен успешно", token);
    }
}
