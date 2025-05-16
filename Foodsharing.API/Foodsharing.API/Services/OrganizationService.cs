using Foodsharing.API.Constants;
using Foodsharing.API.DTOs;
using Foodsharing.API.Interfaces.Repositories;
using Foodsharing.API.Interfaces.Services;
using Foodsharing.API.Models;

namespace Foodsharing.API.Services;

public class OrganizationService : IOrganizationService
{
    private readonly IOrganizationRepository _organizationRepository;
    private readonly IStatusesRepository _statusesRepository;
    private readonly IAddressService _addressService;
    private readonly IImageService _imageService;
    private readonly IStringGenerator _stringGenerator;
    private readonly IUserService _userService;

    public OrganizationService(IOrganizationRepository organizationRepository,
                               IStatusesRepository statusesRepository,
                               IAddressService addressService,
                               IImageService imageService,
                               IStringGenerator stringGenerator,
                               IUserService userService)
    {
        _organizationRepository = organizationRepository;
        _statusesRepository = statusesRepository;
        _addressService = addressService;
        _imageService = imageService;
        _stringGenerator = stringGenerator;
        _userService = userService;
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

    public async Task<LoginDTO> CreateRepresentativeOrganizationAsync(Guid orgId, CancellationToken cancellationToken)
    {
        var org = await GetOrgNameByIdAsync(orgId, cancellationToken);

        var representativeRegData = new RegisterDTO
        {
            FirstName = "Имя",
            Bio = $"Представитель организации {org}",
            UserName = _stringGenerator.GenerateRandomString(8),
            Password = _stringGenerator.GenerateRandomString(8),
        };

        var resReg = await _userService.RegisterAsync(representativeRegData, RolesConsts.RepresentativeOrganization, cancellationToken);

        var user = await _userService.GetByUserNameAsync(representativeRegData.UserName, cancellationToken);

        await _userService.AddRepresentativeAsync(user.Id, orgId, cancellationToken);

        if (resReg.Success)
        {
            return new LoginDTO
            {
                UserName = representativeRegData.UserName,
                Password = representativeRegData.Password
            };
        }

        return new LoginDTO();
    }

    public async Task<OrganizationDTO?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var org = await _organizationRepository.GetOrganizationByIdAsync(id, cancellationToken);

        return org is null ? null : new OrganizationDTO
        {
            Id = org.Id,
            Name = org.Name,
            Address = new AddressDTO
            {
                Region = org.Address.Region,
                City = org.Address.City,
                Street = org.Address.Street,
                House = org.Address.House
            },
            Phone = org.Phone,
            Email = org.Email,
            Website = org.Website,
            Description = org.Description,
            OrganizationForm = org.OrganizationForm.OrganizationFormFullName,
            OrganizationStatus = org.OrganizationStatus.Name,
            LogoImage = org.LogoImage,
        };


    }
    public async Task<IEnumerable<UserDTO>?> GetRepresentativesByOrgIdAsync(Guid orgId, CancellationToken cancellationToken)
    {
        var representatives = await _organizationRepository.GetRepresentativesByOrgIdAsync(orgId, cancellationToken);

        return representatives is null ? null : representatives.Select(
            r => new UserDTO
            {
                UserId = r.User.Id,
                UserName = r.User.UserName,
                FirstName = r.User.Profile?.FirstName,
                LastName = r.User.Profile?.LastName,
                Image = r.User.Profile?.Image
            }).ToList();
    }
}
