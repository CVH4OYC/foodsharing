using System.ComponentModel.DataAnnotations;

namespace Foodsharing.API.Models;

/// <summary>
/// Оценка обмена пользователем
/// </summary>
public class Rating
{
    /// <summary>
    /// Id
    /// </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// Id оцениваемого обмена
    /// </summary>
    [Required]
    public Guid TransactionId { get; set; }

    /// <summary>
    /// Оцениваемый обмен
    /// </summary>
    public Transaction Transaction { get; set; } = null!;

    /// <summary>
    /// Id оценивающего пользователя
    /// </summary>
    [Required]
    public Guid RaterId { get; set; }

    /// <summary>
    /// Оценивающий пользователь
    /// </summary>
    public User Rater { get; set; } = null!;

    /// <summary>
    /// Id оцениваемого пользователя
    /// </summary>
    [Required]
    public Guid RatedUserId { get; set; }

    /// <summary>
    /// Оцениваемый пользователь
    /// </summary>
    public User RatedUser { get; set; } = null!;

    /// <summary>
    /// Оценка
    /// </summary>
    [Required]
    public int Grade { get; set; }

    /// <summary>
    /// Комментарий
    /// </summary>
    public string? Comment { get; set; }
}
