using System.ComponentModel.DataAnnotations;
using Foodsharing.API.Extensions.Attributes;

namespace Foodsharing.API.DTOs.Announcement;

public class AnnouncementDTO
{
    public Guid AnnouncementId { get; set; }
    /// <summary>
    /// Заголовок объявления
    /// </summary>
    [Required]
    [StringLength(50, ErrorMessage = "Длина заголовка превышает 50 символов!")]
    public string Title { get; set; }

    /// <summary>
    /// Описание объявления
    /// </summary>
    [StringLength(500, ErrorMessage = "Длина описания превышает 500 символов!")]
    public string? Description { get; set; }

    public AddressDTO Address { get; set; } = null!;

    /// <summary>
    /// Дата создания объявления
    /// </summary>
    [Required]
    public DateTime DateCreation { get; set; }

    /// <summary>
    /// Срок годности продукта из объявления
    /// </summary>
    [Required]
    public DateTime ExpirationDate { get; set; }

    /// <summary>
    /// Путь к картинке
    /// </summary>
    [Required]
    public string Image { get; set; }

    public CategoryForAnnouncement Category { get; set; } = null!;

    public UserDTO User { get; set; } = null!;

    public string Status { get; set; } = null!;

    public bool IsBookedByCurrentUser { get; set; } 
}
