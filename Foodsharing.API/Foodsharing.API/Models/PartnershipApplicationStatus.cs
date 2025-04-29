using System.ComponentModel.DataAnnotations;

namespace Foodsharing.API.Models;

/// <summary>
/// Статус заявки на партнёрство
/// </summary>
public class PartnershipApplicationStatus
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
    /// Навигационное свойство для связи с PartnershipApplication 
    /// </summary>
    public List<PartnershipApplication>? PartnershipApplications { get; set; }
}
