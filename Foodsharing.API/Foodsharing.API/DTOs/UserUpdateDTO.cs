using Foodsharing.API.Extensions.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Foodsharing.API.DTOs;

public class UserUpdateDTO
{
    [Required]
    public Guid UserId { get; set; }

    [Required]
    public string FirstName { get; set; } = null!;

    public string? LastName { get; set; }

    [AllowedExtensions(new[] { ".jpg", ".jpeg", ".png" }, ErrorMessage = "Допустимые форматы: JPG, JPEG, PNG")]
    public IFormFile? ImageFile { get; set; }

    public string? Bio { get; set; }
}
