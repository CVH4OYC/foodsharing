namespace Foodsharing.API.DTOs;

public class RegisterDTO
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string UserName { get; set; } = null!;
}
