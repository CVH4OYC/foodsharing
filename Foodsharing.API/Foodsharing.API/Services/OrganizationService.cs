using Foodsharing.API.Interfaces;
using Foodsharing.API.Models;

namespace Foodsharing.API.Services;

public class OrganizationService : IOrganizationService
{
    private readonly IOrganizationRepository _organizationRepository;

    public OrganizationService(IOrganizationRepository organizationRepository)
    {
        _organizationRepository = organizationRepository;
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


}
