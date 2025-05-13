using Foodsharing.API.DTOs;
using Foodsharing.API.Models;

namespace Foodsharing.API.Interfaces;

public interface IOrganizationService
{
    Task<List<OrganizationForm>> GetOrgFormsAsync(CancellationToken cancellationToken);

    Task AddAsync(Organization organization, CancellationToken cancellationToken);

    Task<OrganizationForm?> GetOrgFormByNameAsync(string name, CancellationToken cancellationToken);

    Task ActivateOrganizationAsync(Guid orgId, CancellationToken cancellationToken);

    Task<Organization> CreateOrganizationAsync(CreatePartnershipApplicationDTO dto, CancellationToken cancellationToken);

    Task<string> GetOrgNameByIdAsync(Guid orgId, CancellationToken cancellationToken);
}
