using System.ComponentModel.DataAnnotations;
using Foodsharing.API.Extensions.Attributes;

namespace Foodsharing.API.DTOs;

public class RegisterDTO
{
    [Required]
    public string FirstName { get; set; } = null!;

    public string? LastName { get; set; }

    [AllowedExtensions(new[] { ".jpg", ".jpeg", ".png" }, ErrorMessage = "Допустимые форматы: JPG, JPEG, PNG")]
    public IFormFile? ImageFile { get; set; }

    public string? Bio { get; set; }

    [Required]
    public string Password { get; set; } = null!;

    [Required]
    public string UserName { get; set; } = null!;


}
