namespace Foodsharing.API.DTOs.ChatAndMessage;

public class CreateMessageDTO
{
    public Guid ChatId { get; set; }
    public string Text { get; set; } = string.Empty;
    public IFormFile? Image { get; set; }
    public IFormFile? File { get; set; }
}
