using System.ComponentModel.DataAnnotations;
using Foodsharing.API.Abstract;

namespace Foodsharing.API.Models;

/// <summary>
/// Объявление
/// </summary>
public class Announcement : EntityBase
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
    
    /// <summary>
    /// Внешний ключ, указывающий на адрес
    /// </summary>
    [Required]
    public Guid AddressId { get; set; }
    
    /// <summary>
    /// Навигационное свойство для связи с таблицей Address
    /// </summary>
    public Address Address { get; set; } = null!;
    
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
    
    /// <summary>
    /// Внешний ключ, указывающий на категорию продукта питания, указанного в объявлении
    /// </summary>
    [Required]
    public Guid CategoryId { get; set; }

    /// <summary>
    /// Навигационное свойство для связи с таблицей Category
    /// </summary>
    public Category Category { get; set; } = null!;
    
    /// <summary>
    /// Внешний ключ, указывающий на пользователя, который создал объявление
    /// </summary>
    [Required]
    public Guid UserId { get; set; }
    
    /// <summary>
    /// Навигационное свойство для связи с таблицей User
    /// </summary>
    public User User { get; set; } = null!;

    /// <summary>
    /// Навигационное свойство для связи с таблицей Transaction
    /// </summary>
    public List<Transaction>? Transactions { get; set; }
}
