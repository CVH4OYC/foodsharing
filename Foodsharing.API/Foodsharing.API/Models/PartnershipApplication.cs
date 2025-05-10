using System.ComponentModel.DataAnnotations;
using Foodsharing.API.Abstract;

namespace Foodsharing.API.Models;

/// <summary>
/// Заявка на партнёрство
/// </summary>
public class PartnershipApplication : EntityBase
{
    /// <summary>
    /// Id организации, подающей заявку
    /// </summary>
    [Required]
    public Guid OrganizationId { get; set; }

    /// <summary>
    /// Нав. св-во для связи с Organization
    /// </summary>
    public Organization Organization { get; set; } = null!;

    /// <summary>
    /// Номер телефона представителя организации или организации, подающей заявку
    /// </summary>
    [Required]
    public string Phone { get; set; }

    /// <summary>
    /// Адрес электронной почты представителя организации или организации, подающей заявку
    /// </summary>
    [RegularExpression(@"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
    @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$", ErrorMessage = "Неверный формат Email!")]
    [Required]
    public string Email { get; set; }

    /// <summary>
    /// Дата подачи заявки
    /// </summary>
    [Required]
    public DateTime SubmittedAt  { get; set; }

    /// <summary>
    /// Id статуса заявки
    /// </summary>
    [Required]
    public Guid StatusId { get; set; }

    /// <summary>
    /// Статус заявки
    /// </summary>
    [Required]
    public PartnershipApplicationStatus Status { get; set; } = null!;

    /// <summary>
    /// Id модератора/админа, которым рассмотрена заявка
    /// </summary>
    public Guid? ReviewedById { get; set; }

    /// <summary>
    /// Модератор/админ
    /// </summary>
    public User? ReviewedBy { get; set; }
    
    /// <summary>
    /// Дата рассмотрения
    /// </summary>
    public DateTime? ReviewedAt { get; set; }

    /// <summary>
    /// Комментарий проверяющего
    /// </summary>
    public string? Comment { get; set; }
}
