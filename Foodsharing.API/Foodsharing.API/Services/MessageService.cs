using Foodsharing.API.Constants;
using Foodsharing.API.DTOs;
using Foodsharing.API.DTOs.ChatAndMessage;
using Foodsharing.API.Hubs;
using Foodsharing.API.Interfaces.Repositories;
using Foodsharing.API.Interfaces.Services;
using Foodsharing.API.Models;
using Microsoft.AspNetCore.SignalR;

namespace Foodsharing.API.Services;

public class MessageService : IMessageService
{
    private readonly IMessageRepository _messageRepository;
    private readonly IImageService _imageService;
    private readonly IFileService _fileService;
    private readonly IStatusesRepository _statusesRepository;
    private readonly IHubContext<ChatHub> _hubContext;
    private readonly IUserService _userService;

    public MessageService(
        IMessageRepository messageRepository,
        IImageService imageService,
        IFileService fileService,
        IStatusesRepository statusesRepository,
        IHubContext<ChatHub> hubContext,
        IUserService userService)
    {
        _messageRepository = messageRepository;
        _imageService = imageService;
        _fileService = fileService;
        _statusesRepository = statusesRepository;
        _hubContext = hubContext;
        _userService = userService;
    }

    public async Task SendMessageAsync(Guid senderId, CreateMessageDTO dto, CancellationToken cancellationToken)
    {
        var status = await _statusesRepository.GetMessageStatusByNameAsync(MessageStatusesConsts.IsNotRead);
        if (status == null)
            throw new Exception("Статус по умолчанию не найден");

        string? savedImagePath = null;
        string? savedFilePath = null;

        if (dto.Image != null)
            savedImagePath = await _imageService.SaveImageAsync(dto.Image, PathsConsts.ChatImagesFolder);

        if (dto.File != null)
            savedFilePath = await _fileService.SaveFileAsync(dto.File, PathsConsts.ChatFilesFolder);

        var message = new Message
        {
            ChatId = dto.ChatId,
            SenderId = senderId,
            Text = dto.Text,
            Date = DateTime.UtcNow,
            StatusId = status.Id,
            Image = savedImagePath,
            File = savedFilePath
        };

        await _messageRepository.AddAsync(message, cancellationToken);

        var sender = await _userService.GetUserProfileAsync(senderId, cancellationToken);
        if (sender == null)
            throw new Exception("Пользователь не найден");

        var messageDto = new MessageDTO
        {
            Id = message.Id,
            ChatId = dto.ChatId,
            Text = message.Text,
            Date = message.Date,
            Status = status.Name,
            Image = savedImagePath,
            File = savedFilePath,
            Sender = new UserDTO
            {
                UserId = sender.UserId,
                UserName = sender.User.UserName,
                FirstName = sender.FirstName,
                LastName = sender.LastName,
                Image = sender.Image
            }
        };

        // Отправляем сообщение всем в SignalR-группе по chatId
        await _hubContext.Clients.Group(dto.ChatId.ToString())
            .SendAsync("ReceiveMessage", messageDto, cancellationToken);
    }

    public async Task MarkMessagesAsReadAsync(Guid chatId, Guid readerId)
    {
        var readStatus = await _statusesRepository.GetMessageStatusByNameAsync(MessageStatusesConsts.IsRead);
        if (readStatus == null)
            throw new Exception($"Статус {MessageStatusesConsts.IsRead} не найден");

        var messagesToUpdate = await _messageRepository.GetUnreadMessagesForChatAsync(
            chatId, readerId, readStatus.Id);

        foreach (var msg in messagesToUpdate)
        {
            msg.StatusId = readStatus.Id;
        }

        if (messagesToUpdate.Count > 0)
            await _messageRepository.UpdateRangeAsync(messagesToUpdate);
    }


}
