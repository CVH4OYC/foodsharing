﻿using Foodsharing.API.DTOs;
using Microsoft.AspNetCore.Mvc;
using Foodsharing.API.Constants;
using Foodsharing.API.Services;
using Microsoft.AspNetCore.Authorization;
using Foodsharing.API.Infrastructure;
using System.Security.Claims;
using Foodsharing.API.Interfaces.Services;
using Foodsharing.API.Extensions;

namespace Foodsharing.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    /// <summary>
    /// Конструктор контроллера пользователей
    /// </summary>
    public UserController(IUserService userService, IHttpContextAccessor httpContextAccessor)
    {
        _userService = userService;
        _httpContextAccessor = httpContextAccessor;
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

    private void AddCookie(OperationResult result)
    {
        Response.Cookies.Append("token", result.Data, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Lax,
            Expires = DateTimeOffset.UtcNow.AddHours(1),
            Path = "/"
        });
    }

    [HttpPost("logout")]
    [Authorize]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("token", new CookieOptions
        {
            Secure = false,
            SameSite = SameSiteMode.Lax,
            Path = "/"
        });
        return Ok("Вы вышли из системы");
    }

    [HttpPut("profile")]
    [Authorize]
    public async Task<IActionResult> UpdateProfileAsync([FromForm]UserUpdateDTO userDTO, CancellationToken cancellationToken)
    {
        var currentUserId = _httpContextAccessor.HttpContext.User.GetUserId();

        if (currentUserId != userDTO.UserId)
            Unauthorized("Нельзя редактировать чужой профиль");

        await _userService.UpdateProfileAsync(userDTO, cancellationToken);

        return Ok();
    }

    [HttpGet("me")]
    [Authorize]
    public ActionResult<AuthDTO> GetCurrentUser()
    {
        var userId = User.FindFirst("userId")?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("Не удалось получить userId из токена.");
        }

        var roles = User.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();

        var dto = new AuthDTO
        {
            UserId = userId,
            Roles = roles,
        };
        return Ok(dto);
    }

    [HttpGet("my")]
    [Authorize]
    public async Task<IActionResult> GetMyProfile(CancellationToken cancellationToken)
    {
        var profile = await _userService.GetMyProfileAsync(cancellationToken);

        return Ok(profile);
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetUserProfile(Guid userId, CancellationToken cancellationToken)
    {
        var profile = await _userService.GetOtherProfileAsync(userId, cancellationToken);

        return Ok(profile);
    }

    [HttpPatch("location")]
    [Authorize]
    public async Task<IActionResult> UpdateLocationAsync([FromBody] UpdateLocationRequest request, CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.HttpContext?.User.GetUserId();
        if (userId == null)
            return Unauthorized("Пользователь не авторизован");

        await _userService.UpdateLocationAsync(userId.Value, request.Latitude, request.Longitude, cancellationToken);

        return Ok("Местоположение обновлено");
    }
}
