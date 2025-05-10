using Foodsharing.API.Data;
using Foodsharing.API.Interfaces;
using Foodsharing.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Foodsharing.API.Repository;

public class OrganizationRepository : Repository<Organization>,  IOrganizationRepository
{
    private readonly DbContext context;

    public OrganizationRepository(AppDbContext context) : base(context)
    {
        this.context = context;
    }

    public async Task<OrganizationForm?> GetOrgFormByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        var form = await context.Set<OrganizationForm>()
            .Where(f => f.OrganizationFormShortName == name || f.OrganizationFormFullName == name)
            .FirstOrDefaultAsync(cancellationToken);

        return form;
    }
}
