using Foodsharing.API.DTOs.ChatAndMessage;
using Foodsharing.API.Extensions;
using Foodsharing.API.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Foodsharing.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class MessageController : ControllerBase
{

    private readonly IMessageService _messageService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public MessageController(IMessageService messageService, IHttpContextAccessor httpContextAccessor)
    {
        _messageService = messageService;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> SendMessageAsync(CreateMessageDTO dto, CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.HttpContext?.User.GetUserId();
        if (userId == null)
            return Unauthorized();

        await _messageService.SendMessageAsync((Guid)userId, dto, cancellationToken);
        return Ok();
    }
}
