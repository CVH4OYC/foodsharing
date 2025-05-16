namespace Foodsharing.API.DTOs.Announcement;

public class CategoryForAnnouncement
{
    public Guid? CategoryId { get; set; }

    public string? Name { get; set; }

    public Guid? ParentId { get; set; }

    public string? Color { get; set; }
}