using System.ComponentModel.DataAnnotations;

namespace Foodsharing.API.DTOs;

public class RegisterDTO
{
    [Required]
    public string FirstName { get; set; } = null!;

    public string? LastName { get; set; }

    public IFormFile ImageFile { get; set; }

    public string? Bio { get; set; }

    [Required]
    public string Password { get; set; } = null!;

    [Required]
    public string UserName { get; set; } = null!;
}
