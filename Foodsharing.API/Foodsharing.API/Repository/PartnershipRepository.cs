using Foodsharing.API.Constants;
using Foodsharing.API.Controllers;
using Foodsharing.API.Data;
using Foodsharing.API.DTOs;
using Foodsharing.API.Interfaces;
using Foodsharing.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Foodsharing.API.Repository;

public class PartnershipRepository : Repository<PartnershipApplication>, IPartnershipRepository
{
    private readonly DbContext context;

    public PartnershipRepository(AppDbContext context) : base(context)
    {
        this.context = context;
    }

    public async Task<List<PartnershipApplication>> GetPartnershipApplicationsAsync(
        string? search,
        string? sortBy,
        int page,
        int limit,
        string? statusFilter,
        CancellationToken cancellationToken)
    {
        var query = context.Set<PartnershipApplication>()
            .AsNoTracking()
            .Include(p => p.Organization)
                .ThenInclude(o => o.OrganizationForm)
            .Include(p => p.Status)
            .AsQueryable();


        query = statusFilter switch
        {
            "isPending" => query.Where(p => p.Status.Name == PartnershipApplicationStatusesConsts.IsPending),
            "isReviewed" => query.Where(p => p.Status.Name == PartnershipApplicationStatusesConsts.IsReviewed),
            _ => query
        };

        if (!string.IsNullOrWhiteSpace(search))
        {
            var lowered = search.ToLower();
            query = query.Where(p =>
                p.Organization.Name.ToLower().Contains(lowered));
        }

        query = sortBy switch
        {
            "dateDesc" => query.OrderByDescending(p => p.SubmittedAt),
            "dateAsc" => query.OrderBy(p => p.SubmittedAt),
            "name" => query.OrderBy(p => p.Organization.Name),
            _ => query.OrderByDescending(p => p.SubmittedAt)
        };

        var skip = (page - 1) * limit;
        query = query.Skip(skip).Take(limit);
        Console.WriteLine(query.ToQueryString());
        return await query.ToListAsync(cancellationToken);
    }

    public async Task<PartnershipApplication?> GetPartnershipApplicationByIdAsync(Guid applicationId, CancellationToken cancellationToken)
    {
        return await context.Set<PartnershipApplication>()
            .Include(p => p.Organization)
                .ThenInclude(o => o.OrganizationForm)
            .Include(p => p.Organization)
                .ThenInclude(o => o.Address)
            .Include(p => p.Status)
            .Include(p => p.ReviewedBy)
                .ThenInclude(r => r.Profile)
            .FirstOrDefaultAsync(p => p.Id == applicationId, cancellationToken);
    }
}
