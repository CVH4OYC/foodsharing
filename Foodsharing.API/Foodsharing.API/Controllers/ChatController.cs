using Foodsharing.API.DTOs.ChatAndMessage;
using Foodsharing.API.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
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

    [HttpGet("my")]
    [Authorize]
    public async Task<ActionResult<IEnumerable<ChatDTO>>> GetMyChats(CancellationToken cancellationToken)
    {
        var chats = await _chatService.GetMyChatsAsync(cancellationToken);
        return Ok(chats);
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<Guid>> CreateChatAsync(Guid otherUserId, CancellationToken cancellationToken)
    {
        var chatId = await _chatService.CreateChatWithUserAsync(otherUserId, cancellationToken);

        return Ok(chatId);
    }

    [HttpGet("{chatId}")]
    [Authorize]
    public async Task<ActionResult<ChatWithMessagesDTO>> GetChatWithMessagesByIdAsync(
                                                            Guid chatId,
                                                            [FromQuery] int page = 1,
                                                            [FromQuery] int pageSize = 20,
                                                            [FromQuery] string? search = null,
                                                            CancellationToken cancellationToken = default)
    {
        var chat = await _chatService.GetChatWithMessagesAsync(chatId, page, pageSize, search, cancellationToken);

        if (chat == null)
            return NotFound();

        return Ok(chat);
    }
}
