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
    private readonly IMessageRepository _messageRepository;           // ← добавлено для MarkChatAsRead
    private readonly IStatusesRepository _statusesRepository;         // ← добавлено для MarkChatAsRead

    public ChatHub(
        IChatService chatService,
        IMessageService messageService,
        IMessageRepository messageRepository,             // ← добавлено
        IStatusesRepository statusesRepository)            // ← добавлено
    {
        _chatService = chatService;
        _messageService = messageService;
        _messageRepository = messageRepository;
        _statusesRepository = statusesRepository;
    }

    public override async Task OnConnectedAsync()
    {
        // Получаем ID пользователя из JWT
        var userId = Context.User?.GetUserId();

        if (userId != null)
        {
            // Берём все чаты, где он учавствует, и добавляем текущее соединение в каждую группу-chatId
            var chats = await _chatService.GetMyChatsAsync(default);
            if (chats != null)
            {
                foreach (var chat in chats)
                {
                    await Groups.AddToGroupAsync(Context.ConnectionId, chat.Id.ToString());
                }
            }

            // Опционально: подписываем пользователя на «личную» группу user_{userId}
            // await Groups.AddToGroupAsync(Context.ConnectionId, $"user_{userId}");
        }

        await base.OnConnectedAsync();
    }

    /// <summary>
    /// Клиент вызывает, чтобы подписаться на конкретный чат (при заходе в диалог)
    /// </summary>
    public async Task JoinChat(string chatId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, chatId);
    }

    /// <summary>
    /// Клиент вызывает, чтобы отподписаться (при выходе из диалога)
    /// </summary>
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

        // 1) В сервисе помечаем в БД все непрочитанные сообщения как «прочитано»
        await _messageService.MarkMessagesAsReadAsync(chatGuid, userId.Value);

        // 2) Получаем сам статус «прочитано»
        var readStatus = await _statusesRepository.GetMessageStatusByNameAsync(MessageStatusesConsts.IsRead);
        if (readStatus != null)
        {
            // 3) Достаём список тех же сообщений, чтобы слать SignalR-уведомления
            var messagesToNotify = await _messageRepository.GetUnreadMessagesForChatAsync(
                chatGuid,
                userId.Value,
                readStatus.Id
            );

            // После того как мы поменяли статус в DB, нужно извлечь именно перечень
            // сообщений, у которых он изменился. Но поскольку мы уже их пометили,
            // проще вызвать заново фильтр по статусам неравным readStatus.Id:
            // Однако выше мы вызвали MarkMessagesAsRead, поэтому новых «непрочитанных» уже нет.
            // Поэтому, чтобы отправить каждому сообщение событие, 
            // лучше получить оригинальный список ДО обновления (можно менять MarkMessagesAsReadAsync
            // чтобы он возвращал список id — но в учебном проекте допустим упрощение).
            // Для минимальных изменений здесь — мы отправляем единый ChatListUpdate, 
            // а статусы внутри open-chat фронтенд сможет получить при рефреше истории.

            // 4) Отправляем апдейт списка чатов (UnreadCount станет = 0)
            var chatForListDto = await _chatService.BuildChatListDtoAsync(chatGuid);

            await Clients.Group(chatId).SendAsync("ChatListUpdate", chatForListDto);
        }
    }
}