using Foodsharing.API.Constants;
using Foodsharing.API.DTOs;
using Foodsharing.API.DTOs.Announcement;
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

    public async Task<List<PartnershipApplicationDTO>> GetPartnershipApplicationsAsync(string? search, string? sortBy, int page, int limit, string? statusFilter, CancellationToken cancellationToken)
    {
        var applications = await _partnershipRepository.GetPartnershipApplicationsAsync(search, sortBy, page, limit, statusFilter, cancellationToken);

        return applications.Select(p => new PartnershipApplicationDTO
        {
            Id = p.Id,
            OrganizationId = p.OrganizationId,
            Organization = new OrganizationDTO
            {
                Id = p.OrganizationId,
                Name = p.Organization.Name,
                LogoImage = p.Organization.LogoImage
            },
            SubmittedAt = p.SubmittedAt,
            Status = p.Status.Name,
        }).ToList();
    }

    public async Task<PartnershipApplicationDTO?> GetPartnershipApplicationByIdAsync(Guid applicationId, CancellationToken cancellationToken)
    {
        var application = await _partnershipRepository.GetPartnershipApplicationByIdAsync(applicationId, cancellationToken);

        return application is null ? null : new PartnershipApplicationDTO
        {
            Id = application.Id,
            OrganizationId = application.OrganizationId,
            Organization = new OrganizationDTO
            {
                Id = application.OrganizationId,
                Name = application.Organization.Name,
                AddressId = application.Organization.AddressId,
                Address = new AddressDTO
                {
                    AddressId = application.Organization.AddressId,
                    Region = application.Organization.Address.Region,
                    City = application.Organization.Address.City,
                    Street = application.Organization.Address.Street,
                    House = application.Organization.Address.House
                },
                Phone = application.Organization.Phone,
                Email = application.Organization.Email,
                Website = application.Organization.Website,
                Description = application.Organization.Description,
                OrganizationForm = application.Organization.OrganizationForm.OrganizationFormShortName,
                LogoImage = application.Organization.LogoImage
            },
            Phone = application.Phone,
            Email = application.Email,
            SubmittedAt = application.SubmittedAt,
            Status = application.Status.Name,
            ReviewedAt = application.ReviewedAt,
            ReviewedBy = application.ReviewedBy is null ? null : new UserDTO
            {
                UserId = application.ReviewedBy.Id,
                UserName = application.ReviewedBy.UserName,
                LastName = application.ReviewedBy.Profile.LastName,
                FirstName = application.ReviewedBy.Profile.FirstName,
                Image = application.ReviewedBy.Profile.Image
            },

            Comment = application.Comment
        };
    }
}
