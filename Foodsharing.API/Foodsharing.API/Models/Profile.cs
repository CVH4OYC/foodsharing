using System.ComponentModel.DataAnnotations;

namespace Foodsharing.API.Models;

/// <summary>
/// Профиль пользователя
/// </summary>
public class Profile
{
    /// <summary>
    /// Внешний ключ, указывающий на пользователя, которому принадлежит профиль
    /// </summary>
    [Key]
    [Required]
    public Guid  UserId { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Навигационное свойство для связи с таблицей User
    /// </summary>
    public User? User { get; set; }

    /// <summary>
    /// Имя пользователя
    /// </summary>
    [StringLength(50, ErrorMessage = "Длина имени превышает 50 символов!")]
    [Required]
    public string FirstName { get; set; }
    
    /// <summary>
    /// Фамилия пользователя
    /// </summary>
    [StringLength(50, ErrorMessage = "Длина фамилии превышает 50 символов!")]
    public string? LastName { get; set; }
    
    /// <summary>
    /// Путь к изображению профиля
    /// </summary>
    public string? Image { get; set; }
    
    /// <summary>
    /// Описание пользователя
    /// </summary>
    [StringLength(500, ErrorMessage = "Длина описания превышает 500 символов!")]
    public string? Bio { get; set; }

    /// <summary>
    /// Средняя оценка пользователя
    /// </summary>
    public float? Rating { get; set; }

    /// <summary>
    /// Количество оценок от других пользователей
    /// </summary>
    public long RatingCount { get; set; } = 0;

    /// <summary>
    /// Сколько отдано
    /// </summary>
    public long TotalGiven { get; set; } = 0;

    /// <summary>
    /// Сколько получено
    /// </summary>
    public long TotalRecieved { get; set; } = 0;

    /// <summary>
    /// Широта пользователя
    /// </summary>
    public double? Latitude { get; set; }

    /// <summary>
    /// Долгота пользователя
    /// </summary>
    public double? Longitude { get; set; }

}
