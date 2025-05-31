using System.Threading;
using Foodsharing.API.DTOs;
using Foodsharing.API.Interfaces.Repositories;
using Foodsharing.API.Interfaces.Services;
using Foodsharing.API.Models;

namespace Foodsharing.API.Services;

public class AddressService : IAddressService
{
    private readonly IAddressRepository _addressRepository;
    private readonly IGeocodingService _geocodingService;

    public AddressService(IAddressRepository addressRepository, IGeocodingService geocodingService)
    {
        _addressRepository = addressRepository;
        _geocodingService = geocodingService;
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

        return existingAddress?.Id ?? (await CreateNewAddress(addressDto, cancellationToken)).Id;
    }

    private async Task<Address> CreateNewAddress(AddressDTO dto, CancellationToken cancellationToken)
    {
        if (dto is null)
        {
            throw new ArgumentNullException("Передан пустой адрес");
        }

        var newAddress = new Address
        {
            Region = dto.Region,
            City = dto.City,
            Street = dto.Street,
            House = dto.House
        };

        var coords = await _geocodingService.GetCoordinatesAsync(dto.Region, dto.City, dto.Street, dto.House);
        if (coords is not null)
        {
            newAddress.Latitude = coords.Value.Latitude;
            newAddress.Longitude = coords.Value.Longitude;
        }

        await _addressRepository.AddAsync(newAddress, cancellationToken);
        return newAddress;
    }
}
