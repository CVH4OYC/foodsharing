namespace Foodsharing.API.Interfaces.Services;

public interface IGeocodingService
{
    /// <summary>
    /// Получить координаты по адресу
    /// </summary>
    /// <param name="region"></param>
    /// <param name="city"></param>
    /// <param name="street"></param>
    /// <param name="house"></param>
    /// <returns></returns>
    Task<(double Latitude, double Longitude)?> GetCoordinatesAsync(string region, string city, string street, string house);
}
