using Foodsharing.API.Constants;
using Foodsharing.API.DTOs;
using Foodsharing.API.Interfaces;
using Foodsharing.API.Models;

namespace Foodsharing.API.Services;

public class OrganizationService : IOrganizationService
{
    private readonly IOrganizationRepository _organizationRepository;
    private readonly IStatusesRepository _statusesRepository;
    private readonly IAddressService _addressService;
    private readonly IImageService _imageService;

    public OrganizationService(IOrganizationRepository organizationRepository,
                               IStatusesRepository statusesRepository,
                               IAddressService addressService,
                               IImageService imageService)
    {
        _organizationRepository = organizationRepository;
        _statusesRepository = statusesRepository;
        _addressService = addressService;
        _imageService = imageService;
    }

    public async Task AddAsync(Organization organization, CancellationToken cancellationToken)
    {
        await _organizationRepository.AddAsync(organization, cancellationToken);
    }

    public async Task<OrganizationForm?> GetOrgFormByNameAsync(string name, CancellationToken cancellationToken)
    {
        return await _organizationRepository.GetOrgFormByNameAsync(name, cancellationToken);
    }

    public async Task<List<OrganizationForm>> GetOrgFormsAsync(CancellationToken cancellationToken)
    {
        return await _organizationRepository.GetOrgFromsAsync(cancellationToken);
    }

    public async Task ActivateOrganizationAsync(Guid orgId, CancellationToken cancellationToken)
    {
        var org = await _organizationRepository.GetByIdAsync(orgId, cancellationToken);

        var activeStatus = await _statusesRepository.GetOrganizationStatusByName(OrganizationStatusesConsts.IsActive, cancellationToken);

        org.OrganizationStatusId = activeStatus.Id;

        await _organizationRepository.UpdateAsync(org, cancellationToken);
    }

    public async Task<Organization> CreateOrganizationAsync(CreatePartnershipApplicationDTO dto, CancellationToken cancellationToken)
    {
        var addressId = await _addressService.ProcessAddressAsync(dto.Organization.Address);
        var notActiveStatus = await _statusesRepository.GetOrganizationStatusByName(OrganizationStatusesConsts.IsNotActive, cancellationToken);
        var imagePath = await _imageService.SaveImageAsync(dto.Organization.ImageFile, PathsConsts.AnnouncementsFolder);

        var newOrganization = new Organization
        {
            Name = dto.Organization.Name,
            AddressId = addressId,
            Phone = dto.Organization.Phone,
            Email = dto.Organization.Email,
            Website = dto.Organization.Website,
            Description = dto.Organization.Description,
            OrganizationFormId = dto.Organization.OrganizationFormId,
            OrganizationStatusId = notActiveStatus.Id,
            LogoImage = imagePath
        };

        return newOrganization;
    }

    public async Task<string> GetOrgNameByIdAsync(Guid orgId, CancellationToken cancellationToken)
    {
        var org = await _organizationRepository.GetByIdAsync(orgId, cancellationToken);
        return org.Name;
    }

}
