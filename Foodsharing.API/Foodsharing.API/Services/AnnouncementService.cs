using Foodsharing.API.Constants;
using Foodsharing.API.DTOs.Announcement;
using Foodsharing.API.Infrastructure;
using Foodsharing.API.Interfaces;
using Foodsharing.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace Foodsharing.API.Services;

public class AnnouncementService : IAnnouncementService
{

    private readonly IAnnouncementRepository announcementRepository;
    private readonly IAddressService addressService;

    public AnnouncementService(IAnnouncementRepository announcementRepository, IAddressService addressService)
    {
        this.announcementRepository = announcementRepository;
        this.addressService = addressService;
    }

    public async Task<OperationResult> AddAsync(CreateAnnouncementRequest request, CancellationToken cancellationToken = default)
    {
        var addressId = await addressService.ProcessAddressAsync(request.Address);

        var newAnnouncement = new Announcement
        {
            Title = request.Title,
            Description = request.Description,
            AddressId = addressId,
            CategoryId = request.CategoryId,
            UserId = request.UserId,
            DateCreation = DateTime.UtcNow,
            ExpirationDate = request.ExpirationDate,
            Image = ""
        };

        await announcementRepository.AddAsync(newAnnouncement, cancellationToken);

        return OperationResult.SuccessResult("Объявление добавлено успешно");
    }

    public async Task<IEnumerable<AnnouncementDTO>> GetAnnouncementsAsync(CancellationToken cancellationToken)
    {
        var announcements = await announcementRepository.GetAllAnnouncementsAsync(cancellationToken);
        
        return announcements.Select(a => new AnnouncementDTO
        {
            AnnouncementId = a.Id,
            Title = a.Title,
            Description = a.Description,
            ExpirationDate = a.ExpirationDate,
            DateCreation = a.DateCreation,
            Status = CalculateStatus(a.Transactions),
            Address = new AddressForAnnouncementDTO
            {
                AddressId = a.AddressId,
                Region = a.Address.Region,
                City = a.Address.City,
                Street = a.Address.Street,
                House = a.Address.House
            },
            Category = new CategoryForAnnouncement
            {
                CategoryId = a.CategoryId,
                Name = a.Category.Name,
                Color = a.Category.Color,
                ParentId = a.Category.ParentId,
            },
            User = new UserForAnnouncementDTO
            {
                UserId = a.UserId,
                UserName = a.User.UserName,
                FirstName = "",
                LastName = "",
            }
        });
    }

    public async Task<AnnouncementDTO> GetAnnouncementByIdAsync(Guid announcementId, CancellationToken cancellationToken)
    {
        //return await announcementRepository.GetByIdAsync(announcementId, cancellationToken);
        return new AnnouncementDTO();
    }

    private string CalculateStatus(IEnumerable<Transaction> transactions)
    {
        if (transactions == null || !transactions.Any())
            return AnnouncementStatusesConsts.IsFree;

        var latestTransaction = transactions
            .OrderByDescending(t => t.TransactionDate)
            .FirstOrDefault();

        return latestTransaction?.Status?.Name switch
        {
            TransactionStatusesConsts.IsBooked => AnnouncementStatusesConsts.IsBooked,
            TransactionStatusesConsts.IsCompleted => AnnouncementStatusesConsts.IsCompleted,
            TransactionStatusesConsts.IsCanceled => AnnouncementStatusesConsts.IsFree,
            _ => AnnouncementStatusesConsts.IsCompleted
        };
    }
}
