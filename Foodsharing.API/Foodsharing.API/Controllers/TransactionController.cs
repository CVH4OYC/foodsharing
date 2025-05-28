using Foodsharing.API.DTOs;
using Foodsharing.API.DTOs.Announcement;
using Foodsharing.API.Extensions;
using Foodsharing.API.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Foodsharing.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class TransactionController : ControllerBase
{
    private readonly ITransactionService _transactionService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TransactionController(ITransactionService transactionService, IHttpContextAccessor httpContextAccessor)
    {
        _transactionService = transactionService;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpGet("my")]
    [Authorize]
    public async Task<ActionResult<IEnumerable<TransactionDTO>>> GetUserTransactionsAsync(CancellationToken cancellationToken, bool isSender = true)
    {
        var userId = _httpContextAccessor.HttpContext.User.GetUserId();

        var transaction = await _transactionService.GetUsersTransactionsAsync((Guid)userId, cancellationToken, isSender);

        return Ok(transaction);
    }

    [HttpGet("{transactionId}")]
    [Authorize]
    public async Task<ActionResult<IEnumerable<TransactionDTO>>> GetTransactionByIdAsync(Guid transactionId, CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.HttpContext.User.GetUserId();

        var transaction = await _transactionService.GetTransactionByIdAsync((Guid)userId, transactionId, cancellationToken);

        return Ok(transaction);
    }

    [HttpPost("rate")]
    [Authorize]
    public async Task<ActionResult<IEnumerable<TransactionDTO>>> RateTransactionByIdAsync(RatingDTO ratingDTO, CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.HttpContext.User.GetUserId();

        await _transactionService.RateTransactionByIdAsync((Guid)userId, ratingDTO, cancellationToken);

        return Ok();
    }
}
