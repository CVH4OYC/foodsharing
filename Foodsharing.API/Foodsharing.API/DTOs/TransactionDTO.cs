using System.ComponentModel.DataAnnotations;
using Foodsharing.API.Abstract;
using Foodsharing.API.DTOs.Announcement;

namespace Foodsharing.API.DTOs;

public class TransactionDTO : EntityBase
{
    /// <summary>
    /// Навигационное свойство для связи с пользователем, отдающим продукты, из таблицы User
    /// </summary>
    public UserDTO Sender { get; set; } = null!;

    /// <summary>
    /// Навигационное свойство для связи с получателем продуктов из таблицы User
    /// </summary>
    public UserDTO Recipient { get; set; }

    /// <summary>
    /// Дата последней смены статуса транзакции
    /// </summary>
    [Required]
    public DateTime TransactionDate { get; set; }

    public string Status { get; set; }

    public AnnouncementDTO Announcement { get; set; }

    public OrganizationDTO Organization { get; set; }
}
