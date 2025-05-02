using System.ComponentModel.DataAnnotations;

namespace Foodsharing.API.DTOs.Announcement;

public class UserForAnnouncementDTO
{
    public Guid? UserId { get; set; }

    public string? UserName { get; set; }

    public string FirstName { get; set; }


    public string? LastName { get; set; }
}
