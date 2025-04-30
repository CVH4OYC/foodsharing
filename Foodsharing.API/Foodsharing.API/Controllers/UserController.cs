using Foodsharing.API.DTOs;
using Foodsharing.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Foodsharing.API.Constants;
using Foodsharing.API.Services;
using Foodsharing.API.Interfaces;

namespace Foodsharing.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDTO regDTO, CancellationToken cancellationToken)
    {
        var result = await _userService.RegisterAsync(regDTO, cancellationToken);
        if (result.Success)
        {
            Response.Cookies.Append("token", result.Data, new CookieOptions
            {
                HttpOnly = true,             // защищает от JS доступа
                Secure = true,               // только по HTTPS
                SameSite = SameSiteMode.Strict,
            });

            return Ok(result.Message);
        }
        else
            return BadRequest(result.Message);
    }
}
