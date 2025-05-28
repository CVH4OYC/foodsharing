using System.ComponentModel.DataAnnotations;

namespace Foodsharing.API.DTOs;

public class RatingDTO
{
    /// <summary>
    /// Оценка
    /// </summary>
    [Required]
    public int Grade { get; set; }

    /// <summary>
    /// Комментарий
    /// </summary>
    public string? Comment { get; set; }

    /// <summary>
    /// Id оцениваемого обмена
    /// </summary>
    [Required]
    public Guid TransactionId { get; set; }
}
