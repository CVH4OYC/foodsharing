using Foodsharing.API.Data;
using Foodsharing.API.Infrastructure;
using Foodsharing.API.Interfaces.Services;
using Foodsharing.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Foodsharing.API.Services;

public class GeolocationService : IGeolocationService
{
    private readonly AppDbContext _db;

    public GeolocationService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<User>> GetUsersNearbyAsync(double lat, double lon, double radiusKm, CancellationToken cancellationToken = default)
    {
        var degreeRadius = radiusKm / 111.0;

        var users = await _db.Set<User>()
            .Include(u => u.Profile)
            .Where(u => u.Profile != null &&
                        u.Profile.Latitude != null && u.Profile.Longitude != null &&
                        u.Profile.Latitude >= lat - degreeRadius &&
                        u.Profile.Latitude <= lat + degreeRadius &&
                        u.Profile.Longitude >= lon - degreeRadius &&
                        u.Profile.Longitude <= lon + degreeRadius)
            .ToListAsync(cancellationToken);

        return users
            .Where(u => GeoUtils.CalculateDistanceKm(lat, lon, u.Profile!.Latitude!.Value, u.Profile.Longitude!.Value) <= radiusKm)
            .ToList();
    }

}

