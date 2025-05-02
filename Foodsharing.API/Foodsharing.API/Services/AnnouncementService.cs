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
    private readonly IImageService imageService;

    public AnnouncementService(IAnnouncementRepository announcementRepository, IAddressService addressService, IImageService imageService)
    {
        this.announcementRepository = announcementRepository;
        this.addressService = addressService;
        this.imageService = imageService;
    }

    public async Task<OperationResult> AddAsync(CreateAnnouncementRequest request, CancellationToken cancellationToken = default)
    {
        var addressId = await addressService.ProcessAddressAsync(request.Address);
        var imagePath = await imageService.SaveImageAsync(request.ImageFile, PathsConsts.AnnouncementsFolder);

        var newAnnouncement = new Announcement
        {
            Title = request.Title,
            Description = request.Description,
            AddressId = addressId,
            CategoryId = request.CategoryId,
            UserId = request.UserId,
            DateCreation = DateTime.UtcNow,
            ExpirationDate = request.ExpirationDate,
            Image = imagePath
        };

        await announcementRepository.AddAsync(newAnnouncement, cancellationToken);

        return OperationResult.SuccessResult("Объявление добавлено успешно");
    }

    public async Task<IEnumerable<AnnouncementDTO>> GetAnnouncementsAsync(CancellationToken cancellationToken = default)
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
            Image = a.Image,
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

    public async Task<AnnouncementDTO?> GetAnnouncementByIdAsync(Guid announcementId, CancellationToken cancellationToken = default)
    {
        var announcement = await announcementRepository.GetAnnouncementByIdAsync(announcementId, cancellationToken);

        return announcement is null ? null : new AnnouncementDTO
        {
            AnnouncementId = announcement.Id,
            Title = announcement.Title,
            Description = announcement.Description,
            ExpirationDate = announcement.ExpirationDate,
            DateCreation = announcement.DateCreation,
            Status = CalculateStatus(announcement.Transactions),
            Image = announcement.Image,
            Address = new AddressForAnnouncementDTO
            {
                AddressId = announcement.AddressId,
                Region = announcement.Address.Region,
                City = announcement.Address.City,
                Street = announcement.Address.Street,
                House = announcement.Address.House
            },
            Category = new CategoryForAnnouncement
            {
                CategoryId = announcement.CategoryId,
                Name = announcement.Category.Name,
                Color = announcement.Category.Color,
                ParentId = announcement.Category.ParentId,
            },
            User = new UserForAnnouncementDTO
            {
                UserId = announcement.UserId,
                UserName = announcement.User.UserName,
                FirstName = "",
                LastName = "",
            }
        };
    }

    private static string CalculateStatus(IEnumerable<Transaction> transactions)
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
