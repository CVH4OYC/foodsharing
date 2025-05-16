using Foodsharing.API.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Foodsharing.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class TestController : ControllerBase
{
    [HttpGet("test")]
    [Authorize(Roles = RolesConsts.Admin)]
    public async Task<IActionResult> Test(CancellationToken cancellationToken)
    {
        return Ok();
    }
}
