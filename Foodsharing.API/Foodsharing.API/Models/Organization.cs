using System.ComponentModel.DataAnnotations;
using Foodsharing.API.Abstract;

namespace Foodsharing.API.Models;

/// <summary>
/// Организация
/// </summary>
public class Organization : EntityBase
{
    /// <summary>
    /// Название организации
    /// </summary>
    [StringLength(100, ErrorMessage = "Длина имени превышает 100 символов!")]
    [Required]
    public string Name { get; set; }
    
    /// <summary>
    /// Внешний ключ, указывающий на адрес организации
    /// </summary>
    [Required]
    public Guid AddressId { get; set; }
    
    /// <summary>
    /// Навигационное свойство для связи с таблицей Address
    /// </summary>
    public Address Address { get; set; } = null!;
    
    /// <summary>
    /// Номер телефона организации
    /// </summary>
    [Required]
    public string Phone { get; set; }
    
    /// <summary>
    /// Адрес электронной почты организации
    /// </summary>
    [RegularExpression(@"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
    @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$", ErrorMessage = "Неверный формат Email!")]
    [Required]
    public string Email { get; set; }
    
    /// <summary>
    /// Ссылка на сайт организации
    /// </summary>
    public string? Website { get; set; }

    /// <summary>
    /// Описание организации
    /// </summary>
    [StringLength(500, ErrorMessage = "Длина описания превышает 500 символов!")]
    public string? Description { get; set; }

    /// <summary>
    /// Id формы хозяйственного управления организации (типа АО, ООО, ИП и т.д.)
    /// </summary>
    [Required]
    public Guid OrganizationFormId { get; set; }

    /// <summary>
    /// Навигационное свойство для связи с OrganizationForm
    /// </summary>
    public OrganizationForm OrganizationForm { get; set; } = null!;

    /// <summary>
    /// Id статуса организации
    /// </summary>
    [Required]
    public Guid OrganizationStatusId { get; set; }

    /// <summary>
    /// Навигационное свойство для связи с OrganizationStatus
    /// </summary>
    public OrganizationStatus OrganizationStatus { get; set; } = null!;

    /// <summary>
    /// Навигационное свойство для связи с таблицей RepresentativeOrganization
    /// </summary>
    public List<RepresentativeOrganization>? Representatives { get; set; }
}
