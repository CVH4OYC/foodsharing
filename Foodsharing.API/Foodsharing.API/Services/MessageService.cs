using Foodsharing.API.Constants;
using Foodsharing.API.DTOs.ChatAndMessage;
using Foodsharing.API.Interfaces.Repositories;
using Foodsharing.API.Interfaces.Services;
using Foodsharing.API.Models;

namespace Foodsharing.API.Services;

public class MessageService : IMessageService
{
    private readonly IMessageRepository _messageRepository;
    private readonly IImageService _imageService;
    private readonly IFileService _fileService;
    private readonly IStatusesRepository _statusesRepository;

    public MessageService(IMessageRepository messageRepository, IImageService imageService, IFileService fileService, IStatusesRepository statusesRepository)
    {
        _fileService = fileService;
        _messageRepository = messageRepository;
        _imageService = imageService;
        _statusesRepository = statusesRepository;
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
    }
}
