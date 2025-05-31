using Foodsharing.API.Interfaces.Services;
using System.Globalization;
using System.Text.Json;

namespace Foodsharing.API.Services;

public class NominatimGeocodingService : IGeocodingService
{
    private readonly HttpClient _httpClient;
    private static readonly SemaphoreSlim _rateLimiter = new(1, 1); // только 1 запрос одновременно
    private static DateTime _lastRequestTime = DateTime.MinValue;
    private static readonly TimeSpan _minDelay = TimeSpan.FromSeconds(1.1); // чуть больше 1 сек

    public NominatimGeocodingService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<(double Latitude, double Longitude)?> GetCoordinatesAsync(string region, string city, string street, string house)
    {
        var address = $"{region}, {city}, {street} {house}";
        var url = $"https://nominatim.openstreetmap.org/search?format=json&q={Uri.EscapeDataString(address)}&limit=1&addressdetails=1";

        await _rateLimiter.WaitAsync(); // блокируем до освобождения

        try
        {
            // Подождать, если с прошлого запроса прошло слишком мало времени
            var timeSinceLast = DateTime.UtcNow - _lastRequestTime;
            if (timeSinceLast < _minDelay)
                await Task.Delay(_minDelay - timeSinceLast);

            _lastRequestTime = DateTime.UtcNow;

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("User-Agent", "foodsharing-app");

            var response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode) return null;

            var content = await response.Content.ReadAsStringAsync();
            var results = JsonSerializer.Deserialize<List<NominatimResult>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (results?.FirstOrDefault() is { } result)
            {
                return (double.Parse(result.Lat, CultureInfo.InvariantCulture), double.Parse(result.Lon, CultureInfo.InvariantCulture));
            }

            return null;
        }
        finally
        {
            _rateLimiter.Release(); // разрешаем следующий
        }
    }

    private class NominatimResult
    {
        public string Lat { get; set; }
        public string Lon { get; set; }
    }
}

