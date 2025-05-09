using Foodsharing.API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Foodsharing.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class BookingController : ControllerBase
{
    private readonly IBookingService _bookingService;

    public BookingController(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    [HttpPost("book")]
    [Authorize]
    public async Task<IActionResult> BookAnnouncementAsync(Guid announcementId, CancellationToken cancellationToken)
    {
        var userId = new Guid(User.Claims.First(c => c.Type == "userId").Value);

        var result = await _bookingService.BookAnnouncementAsync(announcementId, cancellationToken);
        if (result.Success)
        {
            return Ok(result.Message);
        }
        return BadRequest(result.Message);
    }

    [HttpPost("unbook")]
    [Authorize]
    public async Task<IActionResult> UnbookAnnouncementAsync(Guid announcementId, CancellationToken cancellationToken)
    {
        var result = await _bookingService.UnbookAnnouncementAsync(announcementId, cancellationToken);
        if (result.Success)
        {
            return Ok(result.Message);
        }
        return BadRequest(result.Message);
    }

    [HttpPost("complete")]
    [Authorize]
    public async Task<IActionResult> CompleteTransactionAsync(Guid announcementId, CancellationToken cancellationToken)
    {
        var result = await _bookingService.CompleteTransactionAsync(announcementId, cancellationToken);
        if (result.Success)
        {
            return Ok(result.Message);
        }
        return BadRequest(result.Message);
    }
}
