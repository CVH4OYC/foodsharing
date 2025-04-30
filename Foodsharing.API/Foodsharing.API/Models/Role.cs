using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Foodsharing.API.Models;

/// <summary>
/// Роль пользователя
/// </summary>
public class Role : IdentityRole<Guid> 
{  

}
