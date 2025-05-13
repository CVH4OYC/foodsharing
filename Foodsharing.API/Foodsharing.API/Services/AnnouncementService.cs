using Foodsharing.API.Constants;
using Foodsharing.API.DTOs;
using Foodsharing.API.DTOs.Announcement;
using Foodsharing.API.Extensions;
using Foodsharing.API.Infrastructure;
using Foodsharing.API.Interfaces;
using Foodsharing.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Foodsharing.API.Services;

public class AnnouncementService : IAnnouncementService
{

    private readonly IAnnouncementRepository announcementRepository;
    private readonly IAddressService addressService;
    private readonly IImageService imageService;
    private readonly IHttpContextAccessor httpContextAccessor;

    public AnnouncementService(IAnnouncementRepository announcementRepository, IAddressService addressService, IImageService imageService, IHttpContextAccessor httpContextAccessor)
    {
        this.announcementRepository = announcementRepository;
        this.addressService = addressService;
        this.imageService = imageService;
        this.httpContextAccessor = httpContextAccessor;
    }

    public async Task<OperationResult> AddAsync(AnnouncemenstCreateUpdRequest request, CancellationToken cancellationToken = default)
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
            ExpirationDate = request.ExpirationDate.ToUniversalTime(),
            Image = imagePath,
        };

        await announcementRepository.AddAsync(newAnnouncement, cancellationToken);

        return OperationResult.SuccessResult("Объявление добавлено успешно");
    }

    public async Task<OperationResult> UpdateAsync(Guid userId, AnnouncemenstCreateUpdRequest request, CancellationToken cancellationToken = default)
    {
        if (request.UserId != userId) 
            return OperationResult.FailureResult("Автор и текущий пользователь не совпадаетг");

        var addressId = await addressService.ProcessAddressAsync(request.Address);
        var imagePath = await imageService.SaveImageAsync(request.ImageFile, PathsConsts.AnnouncementsFolder);

        var newAnnouncement = new Announcement
        {
            Id = request.Id,
            Title =  request.Title,
            Description = request.Description,
            AddressId = addressId,
            CategoryId = request.CategoryId,
            DateCreation = DateTime.UtcNow,
            ExpirationDate = request.ExpirationDate.ToUniversalTime(),
            UserId = request.UserId,
            Image = string.IsNullOrEmpty(imagePath) ? request.ImagePath : imagePath,
        };

        await announcementRepository.UpdateAsync(newAnnouncement, cancellationToken);

        return OperationResult.SuccessResult("Обновление завершено успешно");
    }


    public async Task<IEnumerable<AnnouncementDTO>> GetAnnouncementsAsync(
        Guid? categoryId = null,
        string? search = null,
        string? statusFilter = null, // заменили isBooked на string
        string? sortBy = null,
        int page = 1,
        int limit = 10,
        CancellationToken cancellationToken = default)
    {
        var query = announcementRepository.GetAllAnnouncements();

        if (categoryId.HasValue)
        {
            query = query.Where(a =>
                a.CategoryId == categoryId.Value ||
                a.Category.ParentId == categoryId.Value);
        }

        if (!string.IsNullOrWhiteSpace(search))
        {
            var lowered = search.ToLower();
            query = query.Where(a => a.Title.ToLower().Contains(lowered));
        }

        if (!string.IsNullOrEmpty(statusFilter))
        {
            switch (statusFilter)
            {
                case "booked":
                    query = query.Where(a =>
                        a.Transactions
                            .OrderByDescending(t => t.TransactionDate)
                            .Select(t => t.Status.Name)
                            .FirstOrDefault() == TransactionStatusesConsts.IsBooked);
                    break;

                case "free":
                    query = query.Where(a =>
                        !a.Transactions.Any() ||
                        a.Transactions
                            .OrderByDescending(t => t.TransactionDate)
                            .Select(t => t.Status.Name)
                            .FirstOrDefault() == TransactionStatusesConsts.IsCanceled);
                    break;

                case "all":
                    query = query.Where(a =>
                    a.Transactions
                        .OrderByDescending(t => t.TransactionDate)
                        .Select(t => t.Status.Name)
                        .FirstOrDefault() != TransactionStatusesConsts.IsCompleted);
                    break;

                default:
                    query = query.Where(a =>
                    a.Transactions
                        .OrderByDescending(t => t.TransactionDate)
                        .Select(t => t.Status.Name)
                        .FirstOrDefault() != TransactionStatusesConsts.IsCompleted);
                    break;
            }
        }

        query = sortBy switch
        {
            "title" => query.OrderBy(a => a.Title),
            "experationDate" => query.OrderByDescending(a => a.ExpirationDate),
            "dateCreation" => query.OrderByDescending(a => a.DateCreation),
            _ => query.OrderByDescending(a => a.DateCreation)
        };

        var announcements = await query
            .Skip((page - 1) * limit)
            .Take(limit)
            .ToListAsync(cancellationToken);

        return announcements.Select(a => new AnnouncementDTO
        {
            AnnouncementId = a.Id,
            Title = a.Title,
            Description = a.Description,
            ExpirationDate = a.ExpirationDate,
            DateCreation = a.DateCreation,
            Status = CalculateStatus(a.Transactions),
            Image = a.Image,
            Address = new AddressDTO
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
            User = new UserDTO
            {
                UserId = a.UserId,
                UserName = a.User.UserName,
                FirstName = a.User.Profile.FirstName,
                LastName = a.User.Profile.LastName,
                Image = a.User.Profile.Image
            }
        });
    }


    public async Task<AnnouncementDTO?> GetAnnouncementByIdAsync(Guid announcementId, CancellationToken cancellationToken = default)
    {
        var announcement = await announcementRepository.GetAnnouncementByIdAsync(announcementId, cancellationToken);

        var currentUserId = httpContextAccessor.HttpContext?.User.GetUserId();
        var activeBooking = announcement.Transactions
            .FirstOrDefault(t => t.Status.Name == TransactionStatusesConsts.IsBooked);

        return announcement is null ? null : new AnnouncementDTO
        {
            AnnouncementId = announcement.Id,
            Title = announcement.Title,
            Description = announcement.Description,
            ExpirationDate = announcement.ExpirationDate,
            DateCreation = announcement.DateCreation,
            Status = CalculateStatus(announcement.Transactions),
            Image = announcement.Image,
            Address = new AddressDTO
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
            User = new UserDTO
            {
                UserId = announcement.UserId,
                UserName = announcement.User.UserName,
                FirstName = announcement.User.Profile.FirstName,
                LastName = announcement.User.Profile.LastName,
                Image = announcement.User.Profile.Image
            },
            IsBookedByCurrentUser = activeBooking is not null && activeBooking?.RecipientId == currentUserId
        };
    }

    public async Task<OperationResult> DeleteAnnouncementByIdAsync(Guid announcementId, CancellationToken cancellationToken = default)
    {
        var announcement = new Announcement
        {
            Id = announcementId,
        };

        await announcementRepository.DeleteAsync(announcement);
        return OperationResult.SuccessResult("Объявление удалено");
    }

    /// <summary>
    /// Получить свои объявления
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<IEnumerable<AnnouncementDTO>> GetMyAnnouncmentsAsync(CancellationToken cancellationToken, string? statusFilter = null)
    {
        var currentUserId = httpContextAccessor.HttpContext?.User.GetUserId();

        return await GetUsersAnnouncmentsAsync((Guid)currentUserId, cancellationToken, statusFilter);
    }

    /// <summary>
    /// Получить чужие объявления
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<IEnumerable<AnnouncementDTO>> GetOtherAnnouncmentsAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await GetUsersAnnouncmentsAsync(userId, cancellationToken, statusFilter: "active");
    }

    private async Task<IEnumerable<AnnouncementDTO>> GetUsersAnnouncmentsAsync(Guid userId, CancellationToken cancellationToken, string? statusFilter = null)
    {
        var announcements = await announcementRepository.GetUsersAnnouncementsAsync(userId, statusFilter, cancellationToken);

        return announcements.Select(a => new AnnouncementDTO
        {
            AnnouncementId = a.Id,
            Title = a.Title,
            Description = a.Description,
            ExpirationDate = a.ExpirationDate,
            DateCreation = a.DateCreation,
            Status = CalculateStatus(a.Transactions),
            Image = a.Image,
            Address = new AddressDTO
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
            User = new UserDTO
            {
                UserId = a.UserId,
                UserName = a.User.UserName,
                FirstName = a.User.Profile.FirstName,
                LastName = a.User.Profile.LastName,
                Image = a.User.Profile.Image
            }
        });
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
