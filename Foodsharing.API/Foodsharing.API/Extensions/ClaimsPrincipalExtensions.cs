using System.Security.Claims;

namespace Foodsharing.API.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static Guid? GetUserId(this ClaimsPrincipal user)
    {
        var claim = user.FindFirst("userId");
        return claim != null && Guid.TryParse(claim.Value, out var guid) ? guid : null;
    }
}
