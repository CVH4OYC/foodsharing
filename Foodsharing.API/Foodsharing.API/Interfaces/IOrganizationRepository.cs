using Foodsharing.API.Models;

namespace Foodsharing.API.Interfaces;

public interface IOrganizationRepository : IRepository<Organization>
{
    Task<OrganizationForm> GetOrgFormByNameAsync(string formName, CancellationToken cancellationToken);

    Task<List<OrganizationForm>> GetOrgFromsAsync (CancellationToken cancellationToken);
}
