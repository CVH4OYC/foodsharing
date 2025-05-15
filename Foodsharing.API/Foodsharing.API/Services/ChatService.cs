using Foodsharing.API.DTOs;
using Foodsharing.API.Extensions;
using Foodsharing.API.Interfaces.Repositories;
using Foodsharing.API.Interfaces.Services;
using Foodsharing.API.Models;
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

        return myChats.Select(c => new ChatDTO
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
            Message = new MessageDTO
            {
                IsMy = currentUserId == c.Messages.FirstOrDefault().SenderId,
                Text = c.Messages.FirstOrDefault().Text,
                Date = c.Messages.FirstOrDefault().Date,
                Status = c.Messages.FirstOrDefault().Status.Name,
                Image = c.Messages.FirstOrDefault().Image,
                File = c.Messages.FirstOrDefault().File
            }
        });
    }

    public async Task<Guid> GetOrCreateChatWithUserAsync(Guid otherUserId, CancellationToken cancellationToken)
    {
        var currentUserId = _httpContextAccessor.HttpContext?.User.GetUserId();
        if (currentUserId == null)
        {
            throw new Exception("Пользователь не авторизован");
        }

        var chat = await _chatRepository.GetChatByInterlocutorsIdsAsync(otherUserId, (Guid)currentUserId, cancellationToken);

        if (chat == null)
        {
            chat = new Chat
            {
                FirstUserId = (Guid)currentUserId,
                SecondUserId = otherUserId,
            };

            await _chatRepository.AddAsync(chat, cancellationToken);
        }

        return chat.Id;
    }
}
