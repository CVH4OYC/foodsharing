using Foodsharing.API.Abstract;
using Foodsharing.API.Models;
using System.ComponentModel.DataAnnotations;

namespace Foodsharing.API.DTOs;

public class PartnershipApplicationDTO : EntityBase
{
    /// <summary>
    /// Id организации, подающей заявку
    /// </summary>
    [Required]
    public Guid OrganizationId { get; set; }

    /// <summary>
    /// Нав. св-во для связи с Organization
    /// </summary>
    public OrganizationDTO? Organization { get; set; }

    /// <summary>
    /// Номер телефона представителя организации или организации, подающей заявку
    /// </summary>
    public string? Phone { get; set; }

    /// <summary>
    /// Адрес электронной почты представителя организации или организации, подающей заявку
    /// </summary>
    [RegularExpression(@"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
    @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$", ErrorMessage = "Неверный формат Email!")]
    public string? Email { get; set; }

    /// <summary>
    /// Дата подачи заявки
    /// </summary>
    public DateTime? SubmittedAt { get; set; }

    /// <summary>
    /// Статуса заявки
    /// </summary>
    public string Status { get; set; } = null!;

    /// <summary>
    /// Id модератора/админа, которым рассмотрена заявка
    /// </summary>
    public Guid? ReviewedById { get; set; }

    /// <summary>
    /// Модератор/админ
    /// </summary>
    public UserDTO? ReviewedBy { get; set; }

    /// <summary>
    /// Дата рассмотрения
    /// </summary>
    public DateTime? ReviewedAt { get; set; }

    /// <summary>
    /// Комментарий проверяющего
    /// </summary>
    public string? Comment { get; set; }
}
