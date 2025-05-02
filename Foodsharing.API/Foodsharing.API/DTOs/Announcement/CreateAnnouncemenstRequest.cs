using System.ComponentModel.DataAnnotations;
using Foodsharing.API.Models;

namespace Foodsharing.API.DTOs.Announcement;

public class CreateAnnouncementRequest
{
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

    [Required]
    public AddressForAnnouncementDTO Address { get; set; }

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

    /// <summary>
    /// Внешний ключ, указывающий на категорию продукта питания, указанного в объявлении
    /// </summary>
    [Required]
    public Guid CategoryId { get; set; }

    /// <summary>
    /// Внешний ключ, указывающий на пользователя, который создал объявление
    /// </summary>
    [Required]
    public Guid UserId { get; set; }
}

