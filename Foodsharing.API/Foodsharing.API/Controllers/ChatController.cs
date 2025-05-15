using Foodsharing.API.DTOs;
using Foodsharing.API.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Foodsharing.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ChatController : ControllerBase
{
    private readonly IChatService _chatService;

    public ChatController(IChatService chatService)
    {
        _chatService = chatService;
    }

    /// <summary>
    /// Получить или создать чат с другим пользователем
    /// </summary>
    /// <param name="otherUserId"></param>
    /// <returns></returns>
    [HttpGet("with/{otherUserId}")]
    [Authorize]
    public async Task<ActionResult<Guid>> GetChatIdWithUserAsync(Guid otherUserId, CancellationToken cancellationToken)
    {
        var chatId = await _chatService.GetChatIdWithUserAsync(otherUserId, cancellationToken);
        
        if (chatId == null) 
            return NotFound();
            
        return Ok(chatId);
    }

    // Получить все чаты текущего пользователя
    [HttpGet("my")]
    [Authorize]
    public async Task<ActionResult<IEnumerable<ChatDTO>>> GetMyChats(CancellationToken cancellationToken)
    {
        var chats = await _chatService.GetMyChatsAsync(cancellationToken);
        return Ok(chats);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateChatAsync(Guid otherUserId, CancellationToken cancellationToken)
    {
        var chatId = await _chatService.CreateChatWithUserAsync(otherUserId, cancellationToken);

        return Ok(chatId);
    }
}
