using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;

namespace Foodsharing.API.Models;

/// <summary>
/// Форма хозяйственного общества
/// </summary>
public class OrganizationForm
{
    /// <summary>
    /// Id формы
    /// </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// Короткое название формы (например, ИП)
    /// </summary>
    public string OrganizationFormShortName { get; set; }

    /// <summary>
    /// Полное название формы (например, Индивидуальный предприниматель) 
    /// </summary> 
    public string OrganizationFormFulltName { get; set; }

    /// <summary>
    /// Навигационное свойство для связи с Organization
    /// </summary>
    public List<Organization>? Organizations { get; set; }
}
