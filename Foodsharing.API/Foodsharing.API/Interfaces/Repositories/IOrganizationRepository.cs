using Foodsharing.API.Models;

namespace Foodsharing.API.Interfaces.Repositories;

public interface IOrganizationRepository : IRepository<Organization>
{
    Task<OrganizationForm> GetOrgFormByNameAsync(string formName, CancellationToken cancellationToken);

    Task<List<OrganizationForm>> GetOrgFromsAsync (CancellationToken cancellationToken);

    Task<Organization?> GetOrganizationByIdAsync (Guid id,  CancellationToken cancellationToken);

    Task<IEnumerable<RepresentativeOrganization>> GetRepresentativesByOrgIdAsync(Guid orgId, CancellationToken cancellationToken);
}
