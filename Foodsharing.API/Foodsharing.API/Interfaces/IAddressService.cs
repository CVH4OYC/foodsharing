using Foodsharing.API.DTOs.Announcement;

namespace Foodsharing.API.Interfaces;

public interface IAddressService
{
    Task<Guid> ProcessAddressAsync(AddressForAnnouncementDTO addressDto, CancellationToken cancellationToken = default);
}
