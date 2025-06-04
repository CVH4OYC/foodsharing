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
    private readonly IChatService _chatService;                  // ← добавлено для ChatListUpdate

    public MessageService(
        IMessageRepository messageRepository,
        IImageService imageService,
        IFileService fileService,
        IStatusesRepository statusesRepository,
        IHubContext<ChatHub> hubContext,
        IUserService userService,
        IChatService chatService)                         // ← добавлен параметр
    {
        _messageRepository = messageRepository;
        _imageService = imageService;
        _fileService = fileService;
        _statusesRepository = statusesRepository;
        _hubContext = hubContext;
        _userService = userService;
        _chatService = chatService;                                  // ← сохранён
    }

    public async Task SendMessageAsync(Guid senderId, CreateMessageDTO dto, CancellationToken cancellationToken)
    {
        // 1) Ищем статус «непрочитано»
        var defaultStatus = await _statusesRepository
            .GetMessageStatusByNameAsync(MessageStatusesConsts.IsNotRead);
        if (defaultStatus == null)
            throw new Exception("Статус по умолчанию не найден");

        // 2) Сохраняем вложения (если есть)
        string? savedImagePath = null;
        string? savedFilePath = null;
        if (dto.Image != null)
            savedImagePath = await _imageService.SaveImageAsync(dto.Image, PathsConsts.ChatImagesFolder);
        if (dto.File != null)
            savedFilePath = await _fileService.SaveFileAsync(dto.File, PathsConsts.ChatFilesFolder);

        // 3) Создаём и сохраняем новое сообщение с базовым статусом «непрочитано»
        var message = new Message
        {
            ChatId = dto.ChatId,
            SenderId = senderId,
            Text = dto.Text,
            Date = DateTime.UtcNow,
            StatusId = defaultStatus.Id,
            Image = savedImagePath,
            File = savedFilePath
        };
        await _messageRepository.AddAsync(message, cancellationToken);

        // 4) Собираем DTO для рассылки по SignalR
        var sender = await _userService.GetUserProfileAsync(senderId, cancellationToken);
        if (sender == null)
            throw new Exception("Пользователь не найден");

        var messageDto = new MessageDTO
        {
            Id = message.Id,
            ChatId = dto.ChatId,
            Text = message.Text,
            Date = message.Date,
            Status = defaultStatus.Name,
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

        // 5) Отправляем всем участникам чата событие «новое сообщение»
        await _hubContext.Clients
            .Group(dto.ChatId.ToString())
            .SendAsync("ReceiveMessage", messageDto, cancellationToken);

        // 6) Помечаем сообщение как «доставлено» (delivered) и шлём SignalR-уведомление
        var deliveredStatus = await _statusesRepository
            .GetMessageStatusByNameAsync(MessageStatusesConsts.IsDelivered);
        if (deliveredStatus != null)
        {
            // Обновляем статус в БД
            message.StatusId = deliveredStatus.Id;
            await _messageRepository.UpdateAsync(message, cancellationToken);

            // Шлём всем участникам чата событие MessageStatusUpdate
            await _hubContext.Clients
                .Group(dto.ChatId.ToString())
                .SendAsync("MessageStatusUpdate", new
                {
                    ChatId = dto.ChatId,
                    MessageId = message.Id,
                    NewStatus = deliveredStatus.Name,
                    Timestamp = DateTime.UtcNow
                }, cancellationToken);
        }

        // 7) Отправляем апдейт для списка чатов (ChatListUpdate)
        var chatForListDto = await _chatService
            .BuildChatListDtoAsync(dto.ChatId, cancellationToken);

        await _hubContext.Clients
            .Group(dto.ChatId.ToString())
            .SendAsync("ChatListUpdate", chatForListDto, cancellationToken);
    }

    public async Task MarkMessagesAsReadAsync(Guid chatId, Guid readerId)
    {
        // 1) Ищем статус «прочитано»
        var readStatus = await _statusesRepository
            .GetMessageStatusByNameAsync(MessageStatusesConsts.IsRead);
        if (readStatus == null)
            throw new Exception($"Статус {MessageStatusesConsts.IsRead} не найден");

        // 2) Получаем список непрочитанных сообщений для этого чата и этого читателя
        var messagesToUpdate = await _messageRepository.GetUnreadMessagesForChatAsync(
            chatId,
            readerId,
            readStatus.Id
        );

        // 3) Меняем статус у каждой записи
        foreach (var msg in messagesToUpdate)
        {
            msg.StatusId = readStatus.Id;
        }

        if (messagesToUpdate.Count > 0)
            await _messageRepository.UpdateRangeAsync(messagesToUpdate);
    }
}
