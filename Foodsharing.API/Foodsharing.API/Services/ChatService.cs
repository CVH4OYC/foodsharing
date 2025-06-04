using Foodsharing.API.Constants;
using Foodsharing.API.DTOs;
using Foodsharing.API.DTOs.ChatAndMessage;
using Foodsharing.API.Extensions;
using Foodsharing.API.Interfaces.Repositories;
using Foodsharing.API.Interfaces.Services;
using Foodsharing.API.Models;

namespace Foodsharing.API.Services;

public class ChatService : IChatService
{
    private readonly IChatRepository _chatRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMessageRepository _messageRepository;             
    private readonly IStatusesRepository _statusesRepository;          

    public ChatService(
        IChatRepository chatRepository,
        IHttpContextAccessor httpContextAccessor,
        IMessageRepository messageRepository,                        
        IStatusesRepository statusesRepository)                        
    {
        _chatRepository = chatRepository;
        _httpContextAccessor = httpContextAccessor;
        _messageRepository = messageRepository;
        _statusesRepository = statusesRepository;
    }

    /// <summary>
    /// Получить все чаты текущего пользователя, вместе с последним сообщением и количеством непрочитанных
    /// </summary>
    public async Task<IEnumerable<ChatDTO>?> GetMyChatsAsync(CancellationToken cancellationToken)
    {
        var currentUserId = _httpContextAccessor.HttpContext?.User.GetUserId();
        if (currentUserId == null)
            throw new Exception("Пользователь не авторизован");

        // Беру все чаты с включённым последним сообщением (Include(c => c.Messages.OrderByDescending(...).Take(1)))
        var myChats = await _chatRepository.GetMyChatsAsync((Guid)currentUserId, cancellationToken);

        // Получаем идентификатор статуса «прочитано»
        var readStatus = await _statusesRepository.GetMessageStatusByNameAsync(MessageStatusesConsts.IsRead);

        var result = new List<ChatDTO>();
        foreach (var chat in myChats)
        {
            // Последнее сообщение (Include уже подтянул самое свежее)
            var lastMessage = chat.Messages?.FirstOrDefault();

            // Определяем, кто собеседник (если текущий — FirstUser, то собеседник = SecondUser и наоборот)
            var interlocutor = chat.FirstUserId == currentUserId
                ? chat.SecondUser
                : chat.FirstUser;

            // Считаем, сколько непрочитанных сообщений
            int unreadCount = 0;
            if (readStatus != null)
            {
                var unreadList = await _messageRepository.GetUnreadMessagesForChatAsync(
                    chat.Id,
                    (Guid)currentUserId,
                    readStatus.Id,
                    cancellationToken
                );
                unreadCount = unreadList.Count;
            }

            // Формируем DTO
            var chatDto = new ChatDTO
            {
                Id = chat.Id,
                Interlocutor = new UserDTO
                {
                    UserId = interlocutor.Id,
                    UserName = interlocutor.UserName,
                    FirstName = interlocutor.Profile.FirstName,
                    LastName = interlocutor.Profile.LastName,
                    Image = interlocutor.Profile.Image
                },
                Message = lastMessage == null
                    ? null
                    : new MessageDTO
                    {
                        Id = lastMessage.Id,
                        ChatId = lastMessage.ChatId,
                        Sender = new UserDTO
                        {
                            UserId = lastMessage.Sender.Id,
                            UserName = lastMessage.Sender.UserName,
                            FirstName = lastMessage.Sender.Profile.FirstName,
                            LastName = lastMessage.Sender.Profile.LastName,
                            Image = lastMessage.Sender.Profile.Image
                        },
                        Text = lastMessage.Text,
                        Date = lastMessage.Date,
                        Status = lastMessage.Status?.Name,
                        Image = lastMessage.Image,
                        File = lastMessage.File
                    },
                UnreadCount = unreadCount
            };

            result.Add(chatDto);
        }

        return result;
    }

    /// <summary>
    /// Построить ChatDTO (последнее сообщение + количество непрочитанных) для одного чата
    /// </summary>
    public async Task<ChatDTO> BuildChatListDtoAsync(
        Guid chatId,
        CancellationToken cancellationToken = default)
    {
        // Берём чат вместе с одним самым свежим сообщением (page=1, pageSize=1)
        var chat = await _chatRepository.GetChatWithMessagesAsync(chatId, 1, 1, null, cancellationToken);
        if (chat == null)
            throw new Exception("Чат не найден");

        var currentUserId = _httpContextAccessor.HttpContext?.User.GetUserId();
        if (currentUserId == null)
            throw new Exception("Пользователь не авторизован");

        // Определяем собеседника
        var interlocutor = chat.FirstUserId == currentUserId
            ? chat.SecondUser
            : chat.FirstUser;

        // Получаем последнее сообщение (оно уже в chat.Messages благодаря paging)
        var lastMsg = chat.Messages?.OrderByDescending(m => m.Date).FirstOrDefault();

        // Считаем непрочитанные
        var readStatus = await _statusesRepository.GetMessageStatusByNameAsync(MessageStatusesConsts.IsRead);
        int unreadCount = 0;
        if (readStatus != null)
        {
            var unreadList = await _messageRepository.GetUnreadMessagesForChatAsync(
                chatId,
                (Guid)currentUserId,
                readStatus.Id,
                cancellationToken
            );
            unreadCount = unreadList.Count;
        }

        // Формируем DTO
        var chatDto = new ChatDTO
        {
            Id = chat.Id,
            Interlocutor = new UserDTO
            {
                UserId = interlocutor.Id,
                UserName = interlocutor.UserName,
                FirstName = interlocutor.Profile.FirstName,
                LastName = interlocutor.Profile.LastName,
                Image = interlocutor.Profile.Image
            },
            Message = lastMsg == null
                ? null
                : new MessageDTO
                {
                    Id = lastMsg.Id,
                    ChatId = lastMsg.ChatId,
                    Sender = new UserDTO
                    {
                        UserId = lastMsg.Sender.Id,
                        UserName = lastMsg.Sender.UserName,
                        FirstName = lastMsg.Sender.Profile.FirstName,
                        LastName = lastMsg.Sender.Profile.LastName,
                        Image = lastMsg.Sender.Profile.Image
                    },
                    Text = lastMsg.Text,
                    Date = lastMsg.Date,
                    Status = lastMsg.Status?.Name,
                    Image = lastMsg.Image,
                    File = lastMsg.File
                },
            UnreadCount = unreadCount
        };

        return chatDto;
    }

    /// <summary>
    /// (Оставляем без изменений) Получить чат с историей сообщений для вывода при открытии диалога
    /// </summary>
    public async Task<ChatWithMessagesDTO?> GetChatWithMessagesAsync(
        Guid chatId,
        int page = 1,
        int pageSize = 20,
        string? search = null,
        CancellationToken cancellationToken = default)
    {
        var currentUserId = _httpContextAccessor.HttpContext?.User.GetUserId();
        if (currentUserId == null)
            throw new Exception("Пользователь не авторизован");

        var chat = await _chatRepository.GetChatWithMessagesAsync(chatId, page, pageSize, search, cancellationToken);
        if (chat == null)
            return null;

        if (chat.FirstUserId != currentUserId && chat.SecondUserId != currentUserId)
            throw new Exception("Попытка получения чужого чата с сообщениями");

        var interlocutor = chat.FirstUserId == currentUserId
            ? chat.SecondUser
            : chat.FirstUser;

        return new ChatWithMessagesDTO
        {
            Id = chat.Id,
            Interlocutor = new UserDTO
            {
                UserId = interlocutor.Id,
                UserName = interlocutor.UserName,
                FirstName = interlocutor.Profile.FirstName,
                LastName = interlocutor.Profile.LastName,
                Image = interlocutor.Profile.Image
            },
            Messages = chat.Messages?.Select(m => new MessageDTO
            {
                Id = m.Id,
                Sender = new UserDTO
                {
                    UserId = m.Sender.Id,
                    UserName = m.Sender.UserName,
                    FirstName = m.Sender.Profile.FirstName,
                    LastName = m.Sender.Profile.LastName,
                    Image = m.Sender.Profile.Image
                },
                Text = m.Text,
                Date = m.Date,
                Status = m.Status?.Name,
                Image = m.Image,
                File = m.File
            }) ?? Enumerable.Empty<MessageDTO>()
        };
    }

    /// <summary>
    /// (Оставляем без изменений) Получить/создать чат с указанным пользователем
    /// </summary>
    public async Task<Guid?> GetChatIdWithUserAsync(Guid otherUserId, CancellationToken cancellationToken)
    {
        var currentUserId = _httpContextAccessor.HttpContext?.User.GetUserId();
        if (currentUserId == null)
            throw new Exception("Пользователь не авторизован");

        var chat = await _chatRepository.GetChatByInterlocutorsIdsAsync(otherUserId, (Guid)currentUserId, cancellationToken);
        return chat?.Id;
    }

    public async Task<Guid> CreateChatWithUserAsync(Guid otherUserId, CancellationToken cancellationToken)
    {
        var currentUserId = _httpContextAccessor.HttpContext?.User.GetUserId();
        if (currentUserId == null)
            throw new Exception("Пользователь не авторизован");

        var existingChat = await _chatRepository.GetChatByInterlocutorsIdsAsync(otherUserId, (Guid)currentUserId, cancellationToken);
        if (existingChat != null)
            throw new Exception("У вас уже есть чат с этим пользователем");

        var chat = new Chat
        {
            FirstUserId = (Guid)currentUserId,
            SecondUserId = otherUserId
        };
        await _chatRepository.AddAsync(chat, cancellationToken);
        return chat.Id;
    }
}
