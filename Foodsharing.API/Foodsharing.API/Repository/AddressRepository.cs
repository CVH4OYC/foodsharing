using Foodsharing.API.Data;
using Foodsharing.API.DTOs;
using Foodsharing.API.Interfaces.Repositories;
using Foodsharing.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Foodsharing.API.Repository;

public class AddressRepository : Repository<Address>, IAddressRepository
{
    private readonly DbContext context;

    public AddressRepository(AppDbContext context) : base(context)
    {
        this.context = context;
    }

    public async Task<Address?> GetAddressByNameAsync(string region, string city, string street, string house, CancellationToken cancellationToken)
    {
        return await context.Set<Address>()
            .FirstOrDefaultAsync(a =>
                a.Region == region &&
                a.City == city &&
                a.Street == street &&
                a.House == house);
    }
}
