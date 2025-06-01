using Foodsharing.API.Extensions;
using Foodsharing.API.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Foodsharing.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class NotificationController : ControllerBase
{
    private readonly INotificationService _notificationService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public NotificationController(INotificationService notificationService, IHttpContextAccessor httpContextAccessor)
    {
        _notificationService = notificationService;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpGet]
    public async Task<IActionResult> GetNotifications([FromQuery] string? status = null, CancellationToken cancellationToken = default)
    {
        var userId = _httpContextAccessor.HttpContext?.User.GetUserId();
        if (userId == null)
            return Unauthorized("Пользователь не авторизован");

        var result = await _notificationService.GetUserNotificationsAsync((Guid)userId, status, cancellationToken);
        return Ok(result);
    }

    [HttpPut("{id}/read")]
    public async Task<IActionResult> MarkAsRead(Guid id, CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.HttpContext?.User.GetUserId();
        if (userId == null)
            return Unauthorized("Пользователь не авторизован");

        await _notificationService.MarkAsReadAsync(userId.Value, id, cancellationToken);

        return Ok();
    }

    [HttpPost("read-all")]
    public async Task<IActionResult> MarkAllAsRead(CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.HttpContext?.User.GetUserId();
        if (userId == null)
            return Unauthorized("Пользователь не авторизован");

        await _notificationService.MarkAllAsReadAsync(userId.Value, cancellationToken);
        return Ok(new { message = "Все уведомления помечены как прочитанные" });
    }
}
