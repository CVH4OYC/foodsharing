using Foodsharing.API.Models;

namespace Foodsharing.API.Interfaces;

public interface IOrganizationService
{
    Task<List<OrganizationForm>> GetOrgFormsAsync(CancellationToken cancellationToken);

    Task AddAsync(Organization organization, CancellationToken cancellationToken);

    Task<OrganizationForm?> GetOrgFormByNameAsync(string name, CancellationToken cancellationToken);
}
