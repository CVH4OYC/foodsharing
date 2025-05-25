using Foodsharing.API.Constants;
using Foodsharing.API.DTOs;
using Foodsharing.API.Extensions;
using Foodsharing.API.Interfaces.Services;
using Foodsharing.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Foodsharing.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class OrganizationController : ControllerBase
{
    private readonly IOrganizationService _organizationService;
    private readonly IFavoritesService _favoritesService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public OrganizationController(IOrganizationService organizationService, IHttpContextAccessor httpContextAccessor, IFavoritesService favoritesService)
    {
        _organizationService = organizationService;
        _httpContextAccessor = httpContextAccessor;
        _favoritesService = favoritesService;
    }

    [HttpGet("forms")]
    public async Task<ActionResult<List<OrganizationForm>>> GetOrgFormsAsync(CancellationToken cancellationToken)
    {
        var forms = await _organizationService.GetOrgFormsAsync(cancellationToken);

        return Ok(forms);
    }

    [HttpGet("{orgId}")]
    public async Task<ActionResult<OrganizationDTO>> GetOrgByIdAsync(Guid orgId, CancellationToken cancellationToken)
    {
        var orgDTO = await _organizationService.GetByIdAsync(orgId, cancellationToken);

        return Ok(orgDTO);
    }

    [HttpPost("createRepresentative")]
    [Authorize(Roles = RolesConsts.Admin)]
    public async Task<ActionResult<LoginDTO>> CreateRepresentativeOrganizationAsync(Guid orgId, CancellationToken cancellationToken)
    {
        var res = await _organizationService.CreateRepresentativeOrganizationAsync(orgId, cancellationToken);
        if (res is null)
            return BadRequest("Не удалось создать представителя организации");

        return Ok(res);
    }

    [HttpGet("representatives/{orgId}")]
    public async Task<ActionResult<OrganizationDTO>> GetRepresentativesByOrgIdAsync(Guid orgId, CancellationToken cancellationToken)
    {
        var representatives = await _organizationService.GetRepresentativesByOrgIdAsync(orgId, cancellationToken);

        return Ok(representatives);
    }

    [HttpPost("favorite")]
    [Authorize]
    public async Task<IActionResult> AddFavoriteOrganizationAsync(Guid orgId, CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.HttpContext?.User.GetUserId();
        if (userId == null)
            return Unauthorized();

        await _favoritesService.AddFavoriteOrganizationAsync(orgId, (Guid)userId, cancellationToken);
        return Ok();
    }


    [HttpGet("favorite")]
    [Authorize]
    public async Task<ActionResult<IEnumerable<OrganizationDTO>>> GetFavoriteOrganizationsAsync(CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.HttpContext?.User.GetUserId();
        if (userId == null)
            return Unauthorized();

        var orgs = await _favoritesService.GetFavoriteOrganizationsAsync((Guid)userId, cancellationToken);
        return Ok(orgs);
    }

    [HttpDelete("favorite")]
    [Authorize]
    public async Task<IActionResult> DeleteFavoriteOrganizationAsync(Guid orgId, CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.HttpContext?.User.GetUserId();
        if (userId == null)
            return Unauthorized();

        await _favoritesService.DeleteFavoriteOrganizationAsync(orgId, (Guid)userId, cancellationToken);
        return Ok();
    }
}
