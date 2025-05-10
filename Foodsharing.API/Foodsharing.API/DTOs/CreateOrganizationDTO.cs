using Foodsharing.API.Models;
using System.ComponentModel.DataAnnotations;

namespace Foodsharing.API.DTOs;

public class CreateOrganizationDTO
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
    public AddressDTO Address { get; set; }

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
    public string OrganizationForm { get; set; }
}
