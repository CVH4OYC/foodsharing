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

    public ChatService(IChatRepository chatRepository, IHttpContextAccessor httpContextAccessor)
    {
        _chatRepository = chatRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<IEnumerable<ChatDTO>?> GetMyChatsAsync(CancellationToken cancellationToken)
    {
        var currentUserId = _httpContextAccessor.HttpContext?.User.GetUserId();
        if (currentUserId == null)
        {
            throw new Exception("Пользователь не авторизован");
        }

        var myChats = await _chatRepository.GetMyChatsAsync((Guid)currentUserId, cancellationToken);

        return myChats.Select(c =>
        {
            var lastMessage = c.Messages?.FirstOrDefault();

            var interlocutor = c.FirstUser.Id == currentUserId ? c.SecondUser : c.FirstUser;

            return new ChatDTO
            {
                Id = c.Id,
                Interlocutor = new UserDTO
                {
                    UserId = interlocutor.Id,
                    UserName = interlocutor.UserName,
                    FirstName = interlocutor.Profile.FirstName,
                    LastName = interlocutor.Profile.LastName,
                    Image = interlocutor.Profile.Image
                },
                Message = lastMessage == null ? null : new MessageDTO
                {
                    IsMy = currentUserId == lastMessage.SenderId,
                    Text = lastMessage.Text,
                    Date = lastMessage.Date,
                    Status = lastMessage.Status?.Name,
                    Image = lastMessage.Image,
                    File = lastMessage.File
                }
            };
        });
    }

    public async Task<Guid?> GetChatIdWithUserAsync(Guid otherUserId, CancellationToken cancellationToken)
    {
        var currentUserId = _httpContextAccessor.HttpContext?.User.GetUserId();
        if (currentUserId == null)
        {
            throw new Exception("Пользователь не авторизован");
        }

        var chat = await _chatRepository.GetChatByInterlocutorsIdsAsync(otherUserId, (Guid)currentUserId, cancellationToken);

        if (chat == null)
            return null;

        return chat.Id;
    }

    public async Task<Guid> CreateChatWithUserAsync(Guid otherUserId, CancellationToken cancellationToken)
    {
        var currentUserId = _httpContextAccessor.HttpContext?.User.GetUserId();
        if (currentUserId == null)
        {
            throw new Exception("Пользователь не авторизован");
        }

        var chat = await _chatRepository.GetChatByInterlocutorsIdsAsync(otherUserId, (Guid)currentUserId, cancellationToken);

        if (chat is not null)
        {
            throw new Exception("У вас уже есть чат с этим пользователем");
        }

        chat = new Chat
        {
            FirstUserId = (Guid)currentUserId,
            SecondUserId = otherUserId,
        };

        await _chatRepository.AddAsync(chat, cancellationToken);

        return chat.Id;
    }

    public async Task<ChatWithMessagesDTO?> GetChatWithMessagesAsync(Guid chatId, int page = 1, int pageSize = 20, string? search = null, CancellationToken cancellationToken = default)
    {
        var currentUserId = _httpContextAccessor.HttpContext?.User.GetUserId();
        if (currentUserId == null)
        {
            throw new Exception("Пользователь не авторизован");
        }

        var chat = await _chatRepository.GetChatWithMessagesAsync(chatId, page, pageSize, search, cancellationToken);

        if (chat is null)
            return null;

        if (chat.FirstUserId != currentUserId && chat.SecondUserId != currentUserId)
            throw new Exception("Попытка получения чужого чата с сообщениями");

        var interlocutor = chat.FirstUserId == currentUserId ? chat.SecondUser : chat.FirstUser;

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
                IsMy = m.SenderId == currentUserId,
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
}
