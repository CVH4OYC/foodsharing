namespace Foodsharing.API.DTOs.Announcement;

public class BookAnnouncementDTO
{
    public Guid AnnouncementId {  get; set; }

    /// <summary>
    /// Тот, кто забронировал объявление
    /// </summary>
    public Guid RecipientId { get; set; }
}
