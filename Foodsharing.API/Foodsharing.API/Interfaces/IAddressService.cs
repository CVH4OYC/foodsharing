using Foodsharing.API.DTOs;

namespace Foodsharing.API.Interfaces;

public interface IAddressService
{
    Task<Guid> ProcessAddressAsync(AddressDTO addressDto, CancellationToken cancellationToken = default);
}
