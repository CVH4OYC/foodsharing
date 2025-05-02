using Foodsharing.API.DTOs.Announcement;
using Foodsharing.API.Interfaces;
using Foodsharing.API.Models;
using Microsoft.AspNetCore.Mvc;

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
    public async Task<ActionResult<IEnumerable<AnnouncementDTO>>> GetAnnouncementsAsync(CancellationToken cancellationToken)
    {
        var announcements = await _announcementService.GetAnnouncementsAsync(cancellationToken);
        return Ok(announcements ?? new List<AnnouncementDTO>());
    }

    [HttpGet("{announcementId}")]
    public async Task<ActionResult<Announcement>> GetAnnouncementByIdAsync(Guid announcementId, CancellationToken cancellationToken)
    {
        var announcement = await _announcementService.GetAnnouncementByIdAsync(announcementId, cancellationToken);
        
        return announcement == null
            ? NotFound($"Объявление с ID {announcementId} не найдено")
            : Ok(announcement);
    }

    [HttpPost]
    public async Task<ActionResult> CreateAnnouncement(CreateAnnouncementRequest dto)
    {
        await _announcementService.AddAsync(dto);
        return Ok();
    }
}
