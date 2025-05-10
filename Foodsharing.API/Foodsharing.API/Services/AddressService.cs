using Foodsharing.API.DTOs;
using Foodsharing.API.Interfaces;
using Foodsharing.API.Models;
using Foodsharing.API.Repository;
using Microsoft.EntityFrameworkCore;

namespace Foodsharing.API.Services;

public class AddressService : IAddressService
{
    private readonly IAddressRepository _addressRepository;

    public AddressService(IAddressRepository addressRepository)
    {
        _addressRepository = addressRepository;
    }

    public async Task<Guid> ProcessAddressAsync(AddressDTO addressDto, CancellationToken cancellationToken = default)
    {
        // Если передан существующий GUID
        if (addressDto.AddressId.HasValue)
        {
            var address = await _addressRepository.GetByIdAsync(addressDto.AddressId.Value);

            if (address is not null)
                return addressDto.AddressId.Value;
        }

        // Если адрес новый - проверяем дубликаты
        var existingAddress = await _addressRepository.GetAddressByNameAsync(addressDto.Region, addressDto.City, addressDto.Street, addressDto.House, cancellationToken);

        return existingAddress?.Id ?? (await CreateNewAddress(addressDto)).Id;
    }

    private async Task<Address> CreateNewAddress(AddressDTO dto)
    {
        var newAddress = new Address
        {
            Region = dto.Region,
            City = dto.City,
            Street = dto.Street,
            House = dto.House
        };

        await _addressRepository.AddAsync(newAddress);
        return newAddress;
    }
}
