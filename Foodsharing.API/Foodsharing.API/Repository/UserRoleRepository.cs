using Foodsharing.API.Data;
using Foodsharing.API.Interfaces.Repositories;
using Foodsharing.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Foodsharing.API.Repository;

public class UserRoleRepository : Repository<UserRole>, IUserRoleRepository
{
    private readonly DbContext context;

    public UserRoleRepository(AppDbContext context) : base(context)
    {
        this.context = context;
    }
}