using System.Security.Claims;
using Foodsharing.API.DTOs.Announcement;
using Foodsharing.API.Interfaces;
using Foodsharing.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Foodsharing.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AnnouncementController : ControllerBase
{
    private readonly IAnnouncementService _announcementService;

    public AnnouncementController(IAnnouncementService announcementService)
    {
        _announcementService = announcementService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AnnouncementDTO>>> GetAnnouncementsAsync(
        [FromQuery] Guid? categoryId,
        [FromQuery] string? search,
        [FromQuery] string? sortBy,
        [FromQuery] int page = 1,
        [FromQuery] int limit = 10,
        CancellationToken cancellationToken = default)
    {
        var result = await _announcementService.GetAnnouncementsAsync(
            categoryId, search, sortBy, page, limit, cancellationToken);

        return Ok(result);
    }

    [HttpGet("{announcementId}")]
    public async Task<ActionResult<AnnouncementDTO>> GetAnnouncementByIdAsync(Guid announcementId, CancellationToken cancellationToken)
    {
        var announcement = await _announcementService.GetAnnouncementByIdAsync(announcementId, cancellationToken);

        return announcement == null
            ? NotFound($"Объявление с ID {announcementId} не найдено")
            : Ok(announcement);
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult> CreateAnnouncementAsync(AnnouncemenstCreateUpdRequest dto, CancellationToken cancellationToken)
    {
        await _announcementService.AddAsync(dto, cancellationToken);
        return Ok();
    }

    [HttpPut]
    [Authorize]
    public async Task<ActionResult> UpdateAnnouncementAsync(AnnouncemenstCreateUpdRequest dto, CancellationToken cancellationToken)
    {
        var userId =new Guid (User.Claims.First(c => c.Type == "userId").Value);
        var result = await _announcementService.UpdateAsync(userId, dto, cancellationToken);
        
        if (result.Success)
            return Ok();
        else 
            return BadRequest();
    }

    [HttpDelete]
    [Authorize]
    public async Task<IActionResult> DeleteAsync(Guid announcementId, CancellationToken cancellationToken)
    {
        var result = await _announcementService.DeleteAnnouncementByIdAsync (announcementId, cancellationToken);
        if ( result.Success)
        {
            return Ok(result.Data);
        }
        return BadRequest(result.Data);

    }
}
