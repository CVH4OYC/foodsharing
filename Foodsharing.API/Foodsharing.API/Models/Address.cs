using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Foodsharing.API.Abstract;

namespace Foodsharing.API.Models;

/// <summary>
/// Адрес
/// </summary>
public class Address : EntityBase
{
    /// <summary>
    /// Регион
    /// </summary>
    [Required]
    [StringLength(100, ErrorMessage = "Длина названия региона превышает 100 символов!")]
    public string Region { get; set; }
    /// <summary>
    /// Город
    /// </summary>
    [Required]
    [StringLength(100, ErrorMessage = "Длина названия города превышает 100 символов!")]
    public string City { get; set; }
    /// <summary>
    /// Улица
    /// </summary>

    [Required]
    [StringLength(100, ErrorMessage = "Длина названия улицы превышает 100 символов!")]
    public string Street { get; set; }
    /// <summary>
    /// Номер дома
    /// </summary>
    [Required]
    [StringLength(10, ErrorMessage = "Длина номера дома превышает 10 символов!")]
    public string House { get; set; }

    /// <summary>
    /// Широта
    /// </summary>
    public double? Latitude { get; set; }

    /// <summary>
    /// Долгота
    /// </summary>
    public double? Longitude { get; set; }

    /// <summary>
    /// Навигационное свойство для связи с таблицей Organization
    /// </summary>
    public Organization? Organization { get; set; }
    
    /// <summary>
    /// Навигационное свойство для связи с таблицей Announcement
    /// </summary>
    public List<Announcement>? Announcements { get; set; }
}
