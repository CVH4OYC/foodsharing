using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Foodsharing.API.DTOs;

public class AddressDTO
{
    public Guid? AddressId { get; set; }
    /// <summary>
    /// Регион
    /// </summary>
    [StringLength(100, ErrorMessage = "Длина названия региона превышает 100 символов!")]
    public string? Region { get; set; }
    /// <summary>
    /// Город
    /// </summary>
    [StringLength(100, ErrorMessage = "Длина названия города превышает 100 символов!")]
    public string? City { get; set; }
    /// <summary>
    /// Улица
    /// </summary>

    [StringLength(100, ErrorMessage = "Длина названия улицы превышает 100 символов!")]
    public string? Street { get; set; }
    /// <summary>
    /// Номер дома
    /// </summary>
    [StringLength(10, ErrorMessage = "Длина номера дома превышает 10 символов!")]
    public string? House { get; set; }

    public double? Latitude { get; set; }

    public double? Longitude { get; set; }
}
