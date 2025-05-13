using System.ComponentModel.DataAnnotations;

namespace Foodsharing.API.DTOs;

public class UserDTO
{
    public Guid? UserId { get; set; }

    public string? UserName { get; set; }

    public string FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Image { get; set; }
}
