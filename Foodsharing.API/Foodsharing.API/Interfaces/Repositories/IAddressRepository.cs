using Foodsharing.API.Models;

namespace Foodsharing.API.Interfaces.Repositories;

public interface IAddressRepository : IRepository<Address>
{
    Task<Address?> GetAddressByNameAsync(string region, string city, string street, string house, CancellationToken cancellationToken);
}
