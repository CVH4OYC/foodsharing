using Foodsharing.API.Models;

namespace Foodsharing.API.Interfaces.Services;

public interface IGeolocationService
{
    Task<List<User>> GetUsersNearbyAsync(double lat, double lon, double radiusKm, CancellationToken cancellationToken = default);
}
