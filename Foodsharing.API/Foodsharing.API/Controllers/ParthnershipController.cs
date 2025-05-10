using Foodsharing.API.Constants;
using Foodsharing.API.DTOs;
using Foodsharing.API.DTOs.Announcement;
using Foodsharing.API.Interfaces;
using Foodsharing.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Foodsharing.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ParthnershipController : ControllerBase
{
    private readonly IPartnershipService _partnershipService;

    public ParthnershipController(IPartnershipService partnershipService)
    {
        _partnershipService = partnershipService;
    }

    /// <summary>
    /// Создать заявку на партнёрство
    /// </summary>
    /// <param name="dto"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost("createApplication")]
    public async Task<ActionResult> CreatePartnershipApplicationAsync([FromForm]CreatePartnershipApplicationDTO dto, CancellationToken cancellationToken)
    {
        var result = await _partnershipService.ProccessPartnershipApplicationAsync(dto, cancellationToken);

        return Ok(result.Message);
    }

    [HttpGet("applications")]
    [Authorize(Roles = RolesConsts.Admin)]
    public async Task<ActionResult<List<PartnershipApplicationDTO>>> GetApplicationsAsync(
        [FromQuery] string? search,
        [FromQuery] string? sortBy,
        [FromQuery] int page = 1,
        [FromQuery] int limit = 10,
        [FromQuery] string? statusFilter = null, 
        CancellationToken cancellationToken = default)
    {

        var applications = await _partnershipService.GetPartnershipApplicationsAsync(search, sortBy, page, limit, statusFilter, cancellationToken);
        return Ok(applications);
    }
}
