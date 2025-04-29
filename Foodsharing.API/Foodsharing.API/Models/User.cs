using Microsoft.AspNetCore.Identity;

namespace Foodsharing.API.Models;

/// <summary>
/// Пользователь
/// </summary>
public class User : IdentityUser<Guid>
{
}
