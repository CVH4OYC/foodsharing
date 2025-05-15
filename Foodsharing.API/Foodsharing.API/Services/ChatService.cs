using Foodsharing.API.DTOs;
using Foodsharing.API.Extensions;
using Foodsharing.API.Interfaces.Repositories;
using Foodsharing.API.Interfaces.Services;
using Foodsharing.API.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

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

            return new ChatDTO
            {
                Id = c.Id,
                Interlocutor = new UserDTO
                {
                    UserId = c.SecondUser.Id,
                    UserName = c.SecondUser.UserName,
                    FirstName = c.SecondUser.Profile.FirstName,
                    LastName = c.SecondUser.Profile.LastName,
                    Image = c.SecondUser.Profile.Image
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
}
