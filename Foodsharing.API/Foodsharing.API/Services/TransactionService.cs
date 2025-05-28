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
    private readonly IRatingRepository _ratingRepository;
    private readonly IUserService _userSevice;

    public TransactionService(IHttpContextAccessor httpContextAccessor, ITransactionRepository transactionRepository, IRatingRepository ratingRepository, IUserService userService)
    {
        _httpContextAccessor = httpContextAccessor;
        _transactionRepository = transactionRepository;
        _ratingRepository = ratingRepository;
        _userSevice = userService;
    }

    public async Task<TransactionDTO?> GetTransactionByIdAsync(Guid currentUserId, Guid transactionId, CancellationToken cancellationToken)
    {
        var transaction = await _transactionRepository.GetTransactionByIdAsync(transactionId, cancellationToken);
        if (transaction == null)
            return null;

        if (transaction.SenderId != currentUserId && transaction.RecipientId != currentUserId)
            throw new Exception("Нельзя получить обмены, в которых ты не участвовал");

        var myRating = transaction.Ratings.FirstOrDefault(r => r.RaterId == currentUserId);

        return new TransactionDTO
        {
            Sender = new UserDTO
            {
                UserId = transaction.SenderId,
                UserName = transaction.Sender.UserName,
                Image = transaction.Sender.Profile.Image,
                FirstName = transaction.Sender.Profile.FirstName,
                LastName = transaction.Sender.Profile.LastName,
            },
            Recipient = new UserDTO
            {
                UserId = transaction.RecipientId,
                UserName = transaction.Recipient.UserName,
                Image = transaction.Recipient.Profile.Image,
                FirstName = transaction.Recipient.Profile.FirstName,
                LastName = transaction.Recipient.Profile.LastName,
            },
            Announcement = new DTOs.Announcement.AnnouncementDTO
            {
                Image = transaction.Announcement.Image,
                Title = transaction.Announcement.Title,
                AnnouncementId = transaction.AnnouncementId
            },
            TransactionDate = transaction.TransactionDate,
            Status = transaction.Status.Name,
            Organization = transaction.Sender.Representative != null
        ? new OrganizationDTO
        {
            Id = transaction.Sender.Representative.OrganizationId,
            Name = transaction.Sender.Representative.Organization?.Name,
            LogoImage = transaction.Sender.Representative.Organization?.LogoImage
        }
        : null,
            MyRating = myRating != null ? new RatingDTO
            {
                Comment = myRating.Comment,
                Grade = myRating.Grade,
            } : null
        };
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


        return transactions.Select(t =>
        {
            var myRating = t.Ratings.FirstOrDefault(r => r.RaterId == userId);

            return new TransactionDTO
            {
                Id = t.Id,
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
                    UserId = t.RecipientId,
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
                : null,
                MyRating = myRating != null ? new RatingDTO
                {
                    Comment = myRating.Comment,
                    Grade = myRating.Grade,
                } : null
            };
        });

    }

    public async Task RateTransactionByIdAsync(Guid currentUserId, SetRatingDTO ratingDTO, CancellationToken cancellationToken)
    {
        var transaction = await _transactionRepository.GetTransactionByIdAsync(ratingDTO.TransactionId, cancellationToken);

        var rating = await _ratingRepository.GetRatingByTransactionAndRaterAsync(transaction.Id, currentUserId, cancellationToken);
        if (rating != null)
        {
            throw new Exception("Обмен уже оценён");
        }

        if (currentUserId != transaction.SenderId && currentUserId != transaction.RecipientId)
        {
            throw new Exception("Нельзя оценить обмен, в котором вы не участвовали");
        }

        if(transaction.Status.Name != TransactionStatusesConsts.IsCompleted)
        {
            throw new Exception("Можно оценить только завершенный обмен");
        }
        
        var ratedUserId = currentUserId == transaction.SenderId ? transaction.RecipientId : transaction.SenderId;

        var newRating = new Rating
        {
            TransactionId = transaction.Id,
            RaterId = currentUserId,
            RatedUserId = ratedUserId,
            Grade = ratingDTO.Grade,
            Comment = ratingDTO.Comment
        };

        await _ratingRepository.AddAsync(newRating, cancellationToken);
        await _userSevice.AddRatingForUserAsync(ratedUserId, ratingDTO.Grade, cancellationToken);
    }
}
