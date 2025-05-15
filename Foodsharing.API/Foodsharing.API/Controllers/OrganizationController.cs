using Foodsharing.API.Constants;
using Foodsharing.API.DTOs;
using Foodsharing.API.DTOs.Announcement;
using Foodsharing.API.Interfaces.Services;
using Foodsharing.API.Models;
using Foodsharing.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Foodsharing.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class OrganizationController : ControllerBase
{
    private readonly IOrganizationService _organizationService;

    public OrganizationController(IOrganizationService organizationService)
    {
        _organizationService = organizationService;
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
}
