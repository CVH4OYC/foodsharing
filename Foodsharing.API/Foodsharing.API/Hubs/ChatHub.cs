using Foodsharing.API.Constants;
using Foodsharing.API.Extensions;
using Foodsharing.API.Interfaces.Repositories;
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
            var chats = await _chatService.GetMyChatsAsync(default);
            if (chats != null)
            {
                foreach (var chat in chats)
                {
                    await Groups.AddToGroupAsync(Context.ConnectionId, chat.Id.ToString());
                }
            }
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

    /// <summary>
    /// Клиент вызывает, когда пользователь открыл окно чата и все сообщения считать прочитанными
    /// </summary>
    public async Task MarkChatAsRead(string chatId)
    {
        var userId = Context.User?.GetUserId();
        if (userId == null) return;

        var chatGuid = Guid.Parse(chatId);

        // 1) Помечаем сообщения как „прочитано“ и получаем список их ID
        List<Guid> updatedMessageIds = await _messageService.MarkMessagesAsReadAsync(chatGuid, userId.Value);

        // 2) Для каждого ID шлём MessageStatusUpdate
        foreach (var msgId in updatedMessageIds)
        {
            await Clients.Group(chatId).SendAsync("MessageStatusUpdate", new
            {
                chatId = chatId,
                messageId = msgId.ToString(),
                newStatus = MessageStatusesConsts.IsRead
            });
        }

        // 3) Обновляем и рассылаем карточку чата (ChatListUpdate), чтобы в списке чатов исчез счётчик
        var chatForListDto = await _chatService.BuildChatListDtoAsync(chatGuid);
        await Clients.Group(chatId).SendAsync("ChatListUpdate", chatForListDto);
    }
}