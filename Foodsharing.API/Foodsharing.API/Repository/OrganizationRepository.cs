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

    public async Task<Organization?> GetOrganizationByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await context.Set<Organization>()
            .Include(o => o.Address)
            .Include(o => o.OrganizationForm)
            .Include(o => o.Representatives)
            .Include(o => o.OrganizationStatus)
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
    }

    public async Task<OrganizationForm?> GetOrgFormByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        var form = await context.Set<OrganizationForm>()
            .Where(f => f.OrganizationFormShortName == name || f.OrganizationFormFullName == name)
            .FirstOrDefaultAsync(cancellationToken);

        return form;
    }

    public async Task<List<OrganizationForm>> GetOrgFromsAsync(CancellationToken cancellationToken)
    {
        return await context.Set<OrganizationForm>().ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<RepresentativeOrganization>> GetRepresentativesByOrgIdAsync(Guid orgId, CancellationToken cancellationToken)
    {
        return await context.Set<RepresentativeOrganization>()
            .Include(r => r.User)
                .ThenInclude(u => u.Profile)
            .Where(r => r.OrganizationId == orgId)
            .ToListAsync(cancellationToken);
    }
}
