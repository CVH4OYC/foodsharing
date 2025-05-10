using Foodsharing.API.DTOs.Announcement;
using Foodsharing.API.Interfaces;
using Foodsharing.API.Models;
using Foodsharing.API.Services;
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
}
