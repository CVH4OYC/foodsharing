using Foodsharing.API.Constants;
using Foodsharing.API.DTOs;
using Foodsharing.API.Extensions;
using Foodsharing.API.Interfaces.Repositories;
using Foodsharing.API.Interfaces.Services;
using Foodsharing.API.Models;

namespace Foodsharing.API.Services;

public class TransactionService : ITransactionService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ITransactionRepository _transactionRepository;

    public TransactionService(IHttpContextAccessor httpContextAccessor, ITransactionRepository transactionRepository)
    {
        _httpContextAccessor = httpContextAccessor;
        _transactionRepository = transactionRepository;
    }

    public async Task<IEnumerable<TransactionDTO>> GetUsersTransactionsAsync(Guid userId, CancellationToken cancellationToken, bool isSender = true)
    {
        IEnumerable<Transaction> transactions;
        if (isSender)
        {
            transactions = await _transactionRepository.GetUsersTransactionsAsSenderAsync(userId, cancellationToken);
        }
        else
        {
            transactions = await _transactionRepository.GetUsersTransactionsAsRecipientAsync(userId, cancellationToken);
        }

        return transactions.Select(t => new TransactionDTO
        {
            Sender = new UserDTO
            {
                UserId = t.SenderId,
                UserName = t.Sender.UserName,
                Image = t.Sender.Profile.Image,
                FirstName = t.Sender.Profile.FirstName,
                LastName = t.Sender.Profile.LastName,
            },
            Recipient = new UserDTO
            {
                UserName = t.Recipient.UserName,
                Image = t.Recipient.Profile.Image,
                FirstName = t.Recipient.Profile.FirstName,
                LastName = t.Recipient.Profile.LastName,
            },
            Announcement = new DTOs.Announcement.AnnouncementDTO
            {
                Image = t.Announcement.Image,
                Title = t.Announcement.Title,
                AnnouncementId = t.AnnouncementId
            },
            TransactionDate = t.TransactionDate,
            Status = t.Status.Name,
            Organization = t.Sender.Representative != null
                ? new OrganizationDTO
                {
                    Id = t.Sender.Representative.OrganizationId,
                    Name = t.Sender.Representative.Organization?.Name,
                    LogoImage = t.Sender.Representative.Organization?.LogoImage
                }
                : null
        });

    }
}
