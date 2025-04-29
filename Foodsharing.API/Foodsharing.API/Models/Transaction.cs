using System.ComponentModel.DataAnnotations;
using Foodsharing.API.Abstract;

namespace Foodsharing.API.Models;

/// <summary>
/// Обмен едой
/// </summary>
public class Transaction : EntityBase
{
    /// <summary>
    /// Внешний ключ, указывающий на пользователя, отдающего продукты
    /// </summary>
    [Required]
    public Guid SenderId { get; set; }
    
    /// <summary>
    /// Навигационное свойство для связи с пользователем, отдающим продукты, из таблицы User
    /// </summary>
    public User Sender { get; set; } = null!;
    
    /// <summary>
    /// Внешний ключ, указывающий на пользователя, получающего продукты
    /// </summary>
    [Required]
    public Guid RecipientId { get; set; }
    
    /// <summary>
    /// Навигационное свойство для связи с получателем продуктов из таблицы User
    /// </summary>
    public User Recipient { get; set; }
    
    /// <summary>
    /// Дата последней смены статуса транзакции
    /// </summary>
    [Required]
    public DateTime TransactionDate { get; set; }
    
    /// <summary>
    /// Внешний ключ, указывающий на статус транзакции
    /// </summary>
    [Required]
    public Guid StatusId { get; set; }
    
    /// <summary>
    /// Навигационное свойство для связи с таблицей TransactionStatus
    /// </summary>
    public TransactionStatus Status { get; set; }
    
    /// <summary>
    /// Внешний ключ, указывающий на объявление, с которым связана транзакция
    /// </summary>
    [Required]
    public Guid AnnouncementId { get; set; }
    /// <summary>
    /// Навигационное свойство для связи с таблицей Announcement
    /// </summary>
    public Announcement Announcement { get; set; }
}
