namespace Foodsharing.API.DTOs;

public class UserWithProfileDTO
{
    public Guid? UserId { get; set; }

    public string? UserName { get; set; }

    public string FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Bio {  get; set; }

    public string? Image { get; set; }

    public float? Rating { get; set; }
}
