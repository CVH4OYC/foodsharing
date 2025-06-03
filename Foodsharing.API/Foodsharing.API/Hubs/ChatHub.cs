using Foodsharing.API.Extensions;
using Foodsharing.API.Interfaces.Services;
using Foodsharing.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Foodsharing.API.Hubs;

[Authorize]
public class ChatHub : Hub
{
    private readonly IChatService _chatService;
    private readonly IMessageService _messageService;

    public ChatHub(IChatService chatService, IMessageService messageService)
    {
        _chatService = chatService;
        _messageService = messageService;
    }


    public override async Task OnConnectedAsync()
    {
        var userId = Context.User?.GetUserId();

        if (userId != null)
        {
            var chats = await _chatService.GetMyChatsAsync(CancellationToken.None);

            if (chats != null)
            {
                foreach (var chat in chats)
                {
                    await Groups.AddToGroupAsync(Context.ConnectionId, chat.Id.ToString());
                }
            }

            await Groups.AddToGroupAsync(Context.ConnectionId, $"user:{userId}");
        }

        await base.OnConnectedAsync();
    }


    public async Task JoinChat(string chatId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, chatId);
    }

    public async Task LeaveChat(string chatId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatId);
    }

    public async Task MarkChatAsRead(string chatId)
    {
        var userId = Context.User?.GetUserId();
        if (userId == null) return;

        await _messageService.MarkMessagesAsReadAsync(Guid.Parse(chatId), userId.Value);

        // Явно формируем строки
        var payload = new Dictionary<string, string>
    {
        { "ChatId", chatId },
        { "ReaderId", userId.Value.ToString() }
    };

        await Clients.Group(chatId).SendAsync("MessagesMarkedAsRead", payload);
    }
}
