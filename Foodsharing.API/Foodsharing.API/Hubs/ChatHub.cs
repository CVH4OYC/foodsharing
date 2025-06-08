using Foodsharing.API.Constants;
using Foodsharing.API.Extensions;
using Foodsharing.API.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Foodsharing.API.Hubs
{
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
                foreach (var chat in chats)
                {
                    await Groups.AddToGroupAsync(Context.ConnectionId, chat.Id.ToString());
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

        public async Task MarkChatAsRead(string chatId)
        {
            var userId = Context.User?.GetUserId();
            if (userId == null) return;

            var chatGuid = Guid.Parse(chatId);

            // 1) Помечаем сообщения как прочитанные
            var updatedMessageIds = await _messageService.MarkMessagesAsReadAsync(chatGuid, userId.Value);

            // 2) Рассылаем статус прочтения (если нужно)
            foreach (var msgId in updatedMessageIds)
            {
                await Clients.Group(chatId).SendAsync("MessageStatusUpdate", new
                {
                    chatId,
                    messageId = msgId.ToString(),
                    newStatus = MessageStatusesConsts.IsRead
                });
            }

            // 3) Сигналим всем клиентам, что список чатов надо обновить
            await Clients.All.SendAsync("ChatListUpdate");
        }
    }
}
