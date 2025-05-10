using Foodsharing.API.Models;
using System.ComponentModel.DataAnnotations;

namespace Foodsharing.API.DTOs;

public class CreatePartnershipApplicationDTO
{
    public CreateOrganizationDTO Organization { get; set; }

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
}
