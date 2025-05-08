using Foodsharing.API.DTOs;
using Microsoft.AspNetCore.Mvc;
using Foodsharing.API.Constants;
using Foodsharing.API.Services;
using Foodsharing.API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Foodsharing.API.DTOs.Announcement;
using System.Threading.Tasks;

namespace Foodsharing.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    /// <summary>
    /// Конструктор контроллера пользователей
    /// </summary>
    public UserController(IUserService userService)
    {
        _userService = userService;
    }
    /// <summary>
    /// Метод, обрабатывающий маршрут для регистрации пользователя через <see cref="UserService"/>
    /// </summary>
    /// <param name="userService">Сервис для управления регистрацией и входом пользователей</param>
    /// <param name="request">Запрос, содержащий имя пользователя и пароль</param>
    /// <returns>
    /// Код состояния:<br/>
    /// - 200 OK: если регистрация прошла успешно<br/>
    /// - 400 Bad Request: если регистрация завершилась неудачно
    /// </returns>
    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync(RegisterDTO request, CancellationToken cancellationToken)
    {
        var result = await _userService.RegisterAsync(request, RolesConsts.User, cancellationToken);
        if (result.Success)
        {
            AddCookie(result);
            return Ok(result.Message);
        }
        else
        {
            return BadRequest(result.Message);
        }
    }

    /// <summary>
    /// Метод, обрабатывающий маршрут для входа пользователя по имени пользователя и паролю (добавляет сгенерированный jwt токен в куки)
    /// </summary>
    /// <param name="userService">Сервис для управления регистрацией и входом пользователей</param>
    /// <param name="request">Запрос, содержащий имя пользователя и пароль</param>
    /// <returns>
    /// Код состояния:<br/>
    /// - 200 OK: если вход выполнен успешно<br/>
    /// - 400 Bad Request: если вход завершился неудачно
    /// </returns>
    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync(LoginDTO reguest)
    {
        var result = await _userService.LoginAsync(reguest.UserName, reguest.Password);
        if (result.Success)
        {
            AddCookie(result);
            return Ok(result.Message);
        }
        else
            return BadRequest(result.Message);
    }

    private void AddCookie(Infrastructure.OperationResult result)
    {
        Response.Cookies.Append("token", result.Data);
    }

    [HttpPost("logout")]
    [Authorize]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("token");
        return Ok("Вы вышли из системы");
    }

    [HttpGet("me")]
    [Authorize]
    public IActionResult GetCurrentUser()
    {
        var userId = User.FindFirst("userId")?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("Не удалось получить userId из токена.");
        }

        return Ok(new { userId });
    }

    [HttpGet("my")]
    [Authorize]
    public async Task<IActionResult> GetMyProfile(CancellationToken cancellationToken)
    {
        var profile = await _userService.GetMyProfileAsync(cancellationToken);

        return Ok(profile);
    }

    [HttpGet("{userId}")]
    [Authorize]
    public async Task<IActionResult> GetUserProfile(Guid userId, CancellationToken cancellationToken)
    {
        var profile = await _userService.GetOtherProfileAsync(userId, cancellationToken);

        return Ok(profile);
    }
}
