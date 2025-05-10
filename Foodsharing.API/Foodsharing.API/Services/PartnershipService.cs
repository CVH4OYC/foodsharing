using Foodsharing.API.Constants;
using Foodsharing.API.DTOs;
using Foodsharing.API.Infrastructure;
using Foodsharing.API.Interfaces;
using Foodsharing.API.Models;

namespace Foodsharing.API.Services;

public class PartnershipService : IPartnershipService
{
    private readonly IAddressService _addressService;
    private readonly IPartnershipRepository _partnershipRepository;
    private readonly IUserService _userService;
    private readonly IOrganizationService _organizationService;
    private readonly IStatusesRepository _statusesRepository;
    private readonly IImageService _imageService;

    public PartnershipService(IAddressService addressService,
                              IPartnershipRepository partnershipRepository,
                              IUserService userService,
                              IOrganizationService organizationRepository,
                              IStatusesRepository statusesRepository,
                              IImageService imageService)
    {
        _addressService = addressService;
        _partnershipRepository = partnershipRepository;
        _userService = userService;
        _organizationService = organizationRepository;
        _statusesRepository = statusesRepository;
        _imageService = imageService;
    }

    public async Task<OperationResult> ProccessPartnershipApplicationAsync(CreatePartnershipApplicationDTO dto, CancellationToken cancellationToken)
    {
        Organization newOrganization = await CreateOrganizationAsync(dto, cancellationToken);

        await _organizationService.AddAsync(newOrganization, cancellationToken);

        PartnershipApplication newPartnershipApplication = await CreateParthershipApplication(dto, newOrganization, cancellationToken);

        await _partnershipRepository.AddAsync(newPartnershipApplication, cancellationToken);

        return OperationResult.SuccessResult("Заявка успешно отправлена");
    }

    private async Task<PartnershipApplication> CreateParthershipApplication(CreatePartnershipApplicationDTO dto, Organization newOrganization, CancellationToken cancellationToken)
    {
        var pendingStatus = await _statusesRepository.GetPartnershipApplicationStatusByName(PartnershipApplicationStatusesConsts.IsPending, cancellationToken);

        var newPartnershipApplication = new PartnershipApplication
        {
            OrganizationId = newOrganization.Id,
            Phone = dto.Phone,
            Email = dto.Email,
            SubmittedAt = DateTime.UtcNow,
            StatusId = pendingStatus.Id
        };
        return newPartnershipApplication;
    }

    private async Task<Organization> CreateOrganizationAsync(CreatePartnershipApplicationDTO dto, CancellationToken cancellationToken)
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
}
