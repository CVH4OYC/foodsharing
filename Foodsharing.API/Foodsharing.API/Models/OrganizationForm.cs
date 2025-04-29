using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;
using Foodsharing.API.Abstract;

namespace Foodsharing.API.Models;

/// <summary>
/// Форма хозяйственного общества
/// </summary>
public class OrganizationForm : EntityBase
{
    /// <summary>
    /// Короткое название формы (например, ИП)
    /// </summary>
    public string OrganizationFormShortName { get; set; }

    /// <summary>
    /// Полное название формы (например, Индивидуальный предприниматель) 
    /// </summary> 
    public string OrganizationFormFullName { get; set; }

    /// <summary>
    /// Навигационное свойство для связи с Organization
    /// </summary>
    public List<Organization>? Organizations { get; set; }
}
