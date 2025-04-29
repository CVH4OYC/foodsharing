using System.ComponentModel.DataAnnotations;

namespace Foodsharing.API.Models;

/// <summary>
/// Статус организации в системе
/// </summary>
public class OrganizationStatus
{
    /// <summary>
    /// Id статуса
    /// </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// Название статуса
    /// </summary> 
    public string Name { get; set; }

    /// <summary>
    /// Навигационное свойство для связи с Organization
    /// </summary>
    public List<Organization>? Organizations { get; set; }
}
