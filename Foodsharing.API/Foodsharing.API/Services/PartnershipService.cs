using Foodsharing.API.Constants;
using Foodsharing.API.Controllers;
using Foodsharing.API.DTOs;
using Foodsharing.API.DTOs.Announcement;
using Foodsharing.API.DTOs.Parthner;
using Foodsharing.API.Extensions;
using Foodsharing.API.Infrastructure;
using Foodsharing.API.Interfaces;
using Foodsharing.API.Models;
using static System.Net.Mime.MediaTypeNames;

namespace Foodsharing.API.Services;

public class PartnershipService : IPartnershipService
{
    private readonly IPartnershipRepository _partnershipRepository;
    private readonly IUserService _userService;
    private readonly IOrganizationService _organizationService;
    private readonly IStatusesRepository _statusesRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IStringGenerator _stringGenerator;

    public PartnershipService(IAddressService addressService,
                              IPartnershipRepository partnershipRepository,
                              IUserService userService,
                              IOrganizationService organizationRepository,
                              IStatusesRepository statusesRepository,
                              IImageService imageService,
                              IHttpContextAccessor httpContextAccessor,
                              IStringGenerator stringGenerator)
    {
        _partnershipRepository = partnershipRepository;
        _userService = userService;
        _organizationService = organizationRepository;
        _statusesRepository = statusesRepository;
        _httpContextAccessor = httpContextAccessor;
        _stringGenerator = stringGenerator;
    }

    public async Task<OperationResult> ProccessPartnershipApplicationAsync(CreatePartnershipApplicationDTO dto, CancellationToken cancellationToken)
    {
        Organization newOrganization = await _organizationService.CreateOrganizationAsync(dto, cancellationToken);

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

    public async Task<OperationResult> AcceptApplicationAsync(AcceptApplicationRequest request, CancellationToken cancellationToken)
    {
        var app = await _partnershipRepository.GetByIdAsync(request.applicationId, cancellationToken);

        if (await IsApplicationReview(app, cancellationToken))
            return new OperationResult { Success = false, Message = "Заявка уже рассмотрена" };

        var currentAdminId = _httpContextAccessor.HttpContext?.User.GetUserId();

        var isReviewedStatus = await _statusesRepository.GetPartnershipApplicationStatusByName(PartnershipApplicationStatusesConsts.IsReviewed, cancellationToken);

        app.Comment = request.Comment;
        app.ReviewedById = currentAdminId;
        app.ReviewedAt = DateTime.UtcNow;
        app.StatusId = isReviewedStatus.Id;

        await _partnershipRepository.UpdateAsync(app, cancellationToken);

        await _organizationService.ActivateOrganizationAsync(app.OrganizationId, cancellationToken);

        return new OperationResult { Success = true, Message = "Заявка успешно принята" };
    }

    private async Task<bool> IsApplicationReview(PartnershipApplication app, CancellationToken cancellationToken)
    {
        var reviewedStatus = await _statusesRepository.GetPartnershipApplicationStatusByName(PartnershipApplicationStatusesConsts.IsReviewed, cancellationToken);
        return app.StatusId == reviewedStatus.Id;
    }

    public async Task<OperationResult> RejectApplicationAsync(AcceptApplicationRequest request, CancellationToken cancellationToken)
    {
        var app = await _partnershipRepository.GetByIdAsync(request.applicationId, cancellationToken);

        if (await IsApplicationReview(app, cancellationToken))
            return new OperationResult { Success = false, Message = "Заявка уже рассмотрена" };

        var currentAdminId = _httpContextAccessor.HttpContext?.User.GetUserId();

        var isReviewedStatus = await _statusesRepository.GetPartnershipApplicationStatusByName(PartnershipApplicationStatusesConsts.IsReviewed, cancellationToken);

        app.Comment = request.Comment;
        app.ReviewedById = currentAdminId;
        app.ReviewedAt = DateTime.UtcNow;
        app.StatusId = isReviewedStatus.Id;

        await _partnershipRepository.UpdateAsync(app, cancellationToken);

        return new OperationResult { Success = true, Message = "Заявка успешно отклонена" };
    }

    public async Task<LoginDTO> CreateRepresentativeOrganizationAsync(Guid orgId, CancellationToken cancellationToken)
    {
        var org = await _organizationService.GetOrgNameByIdAsync(orgId, cancellationToken);

        var representativeRegData = new RegisterDTO
        {
            FirstName = "Имя",
            Bio = $"Представитель организации {org}",
            UserName = _stringGenerator.GenerateRandomString(8),
            Password = _stringGenerator.GenerateRandomString(8),
        };

        var resReg = await _userService.RegisterAsync(representativeRegData, RolesConsts.RepresentativeOrganization, cancellationToken);

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
}
