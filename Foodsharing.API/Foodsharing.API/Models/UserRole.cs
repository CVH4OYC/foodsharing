using Microsoft.AspNetCore.Identity;

namespace Foodsharing.API.Models;

/// <summary>
/// Роль пользователя
/// </summary>
public class UserRole : IdentityUserRole<Guid> 
{
}
