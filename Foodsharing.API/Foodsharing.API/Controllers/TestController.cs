using Foodsharing.API.Constants;
using Foodsharing.API.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Foodsharing.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class TestController : ControllerBase
{
    [HttpGet("test")]
    [Authorize]
    public async Task<IActionResult> Test(CancellationToken cancellationToken)
    {
        return Ok();
    }

    [HttpGet("token")]
    public IActionResult GetTokenFromCookie()
    {
        var token = Request.Cookies["token"];  // Извлечение токена из куки
        if (string.IsNullOrEmpty(token))
        {
            return Unauthorized("Token not found in cookies.");
        }

        return Ok(new { Token = token });
    }
}
