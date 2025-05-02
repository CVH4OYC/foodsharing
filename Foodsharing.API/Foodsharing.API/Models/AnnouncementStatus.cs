using Foodsharing.API.Abstract;

namespace Foodsharing.API.Models;

public class AnnouncementStatus : EntityBase
{
    public string Name { get; set; }

    public List<Announcement>? Announcements { get; set; }
}
