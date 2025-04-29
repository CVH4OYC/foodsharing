using System.ComponentModel.DataAnnotations;
using Foodsharing.API.Abstract;

namespace Foodsharing.API.Models;

/// <summary>
/// Статус организации в системе
/// </summary>
public class OrganizationStatus : EntityBase
{
    /// <summary>
    /// Название статуса
    /// </summary> 
    public string Name { get; set; }

    /// <summary>
    /// Навигационное свойство для связи с Organization
    /// </summary>
    public List<Organization>? Organizations { get; set; }
}
