using Foodsharing.API.Data;
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
}
