using System.Runtime.CompilerServices;
using Foodsharing.API.Constants;
using Foodsharing.API.DTOs.Announcement;
using Foodsharing.API.Extensions;
using Foodsharing.API.Infrastructure;
using Foodsharing.API.Interfaces.Repositories;
using Foodsharing.API.Interfaces.Services;
using Foodsharing.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Foodsharing.API.Services;

public class BookingService : IBookingService
{
    private readonly IAnnouncementRepository announcementRepository;
    private readonly ITransactionRepository transactionRepository;
    private readonly IStatusesRepository statusesRepository;
    private readonly IHttpContextAccessor httpContextAccessor;


    public BookingService(IAnnouncementRepository announcementRepository, ITransactionRepository transactionRepository, IStatusesRepository statusesRepository, IHttpContextAccessor httpContextAccessor)
    {
        this.announcementRepository = announcementRepository;
        this.transactionRepository = transactionRepository;
        this.statusesRepository = statusesRepository;
        this.httpContextAccessor = httpContextAccessor;
    }

    public async Task<OperationResult> BookAnnouncementAsync(Guid announcementId, CancellationToken cancellationToken)
    {
        var recipientId = httpContextAccessor.HttpContext?.User.GetUserId();
        if (recipientId == null)
            return OperationResult.FailureResult("Пользователь не авторизован");

        var usersBookings = await transactionRepository.GetUsersBookedTransactions((Guid)recipientId, cancellationToken);
        if (usersBookings.Any())
        {
            return OperationResult.FailureResult("У вас уже есть забронированные продукты. Сначала заберите их.");
        }

        var announcement = await announcementRepository.GetAnnouncementByIdAsync(announcementId, cancellationToken);
        if (announcement == null)
            return OperationResult.FailureResult("Объявление не найдено");

        if (announcement.UserId == recipientId)
        {
            return OperationResult.FailureResult("Нельзя бронировать своё собственное объявление.");
        }

        var statusIsBooked = await statusesRepository.GetTransactionStatusByName(TransactionStatusesConsts.IsBooked, cancellationToken);
        if (statusIsBooked == null)
            return OperationResult.FailureResult("Не удалось получить статус бронирования");

        var lastUserTransaction = announcement.Transactions
            .Where(t => t.RecipientId == recipientId)
            .OrderByDescending(t => t.TransactionDate)
            .FirstOrDefault();

        var isBookedByOther = announcement.Transactions
            .Any(t => t.Status.Name == TransactionStatusesConsts.IsBooked && t.RecipientId != recipientId);

        if (isBookedByOther)
        {
            return OperationResult.FailureResult("Объявление уже кем-то забронировано");
        }

        if (lastUserTransaction != null && lastUserTransaction.Status.Name == TransactionStatusesConsts.IsCanceled)
        {
            lastUserTransaction.StatusId = statusIsBooked.Id;
            lastUserTransaction.TransactionDate = DateTime.UtcNow;

            await transactionRepository.UpdateAsync(lastUserTransaction, cancellationToken);
        }
        else
        {
            // Первичное бронирование — создаём новую транзакцию
            var newTransaction = new Transaction
            {
                AnnouncementId = announcement.Id,
                RecipientId = recipientId.Value,
                SenderId = announcement.UserId,
                TransactionDate = DateTime.UtcNow,
                StatusId = statusIsBooked.Id
            };

            await transactionRepository.AddAsync(newTransaction, cancellationToken);
        }

        return OperationResult.SuccessResult("Продукты забронированы");
    }


    public async Task<OperationResult> UnbookAnnouncementAsync(Guid announcementId, CancellationToken cancellationToken)
    {
        var recipientId = httpContextAccessor.HttpContext?.User.GetUserId();
        if (recipientId == null)
            return OperationResult.FailureResult("Пользователь не авторизован");

        var announcement = await announcementRepository.GetAnnouncementByIdAsync(announcementId, cancellationToken);
        if (announcement == null)
            return OperationResult.FailureResult("Объявление не найдено");

        var status = await statusesRepository.GetTransactionStatusByName(TransactionStatusesConsts.IsCanceled, cancellationToken);
        if (status == null)
            return OperationResult.FailureResult("Статус отмены не найден");

        var transaction = await transactionRepository.GetBookedTransactionByAnnouncementIdAsync(announcementId, cancellationToken);
        if (transaction == null)
            return OperationResult.FailureResult("Бронирование не найдено");

        // Проверяем, что именно этот пользователь делал бронь
        if (transaction.RecipientId != recipientId)
            return OperationResult.FailureResult("Вы не можете отменить бронь, сделанную другим пользователем");

        transaction.StatusId = status.Id;
        transaction.TransactionDate = DateTime.UtcNow;

        await transactionRepository.UpdateAsync(transaction, cancellationToken);

        return OperationResult.SuccessResult("Бронь снята");
    }

    public async Task<OperationResult> CompleteTransactionAsync(Guid announcementId, CancellationToken cancellationToken)
    {
        var sendertId = httpContextAccessor.HttpContext?.User.GetUserId();
        if (sendertId == null)
            return OperationResult.FailureResult("Пользователь не авторизован");

        var announcement = await announcementRepository.GetAnnouncementByIdAsync(announcementId, cancellationToken);
        if (announcement == null)
            return OperationResult.FailureResult("Объявление не найдено");

        var status = await statusesRepository.GetTransactionStatusByName(TransactionStatusesConsts.IsCompleted, cancellationToken);
        if (status == null)
            return OperationResult.FailureResult("Статус завершения не найден");

        var transaction = await transactionRepository.GetBookedTransactionByAnnouncementIdAsync(announcementId, cancellationToken);
        if (transaction == null)
            return OperationResult.FailureResult("Бронирование не найдено");

        // Проверяем, что именно этот пользователь делал бронь
        if (transaction.SenderId != sendertId)
            return OperationResult.FailureResult("Вы не можете завершить обмен, инициатором которого не являетесь");

        transaction.StatusId = status.Id;
        transaction.TransactionDate = DateTime.UtcNow;

        await transactionRepository.UpdateAsync(transaction, cancellationToken);

        return OperationResult.SuccessResult("Обмен завершён, объявление закрыто");
    }

}
