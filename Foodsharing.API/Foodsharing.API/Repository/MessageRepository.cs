using Foodsharing.API.Data;
using Foodsharing.API.Interfaces.Repositories;
using Foodsharing.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Foodsharing.API.Repository;

public class MessageRepository : Repository<Message>, IMessageRepository
{
    private readonly DbContext context;

    public MessageRepository(AppDbContext context) : base(context)
    {
        this.context = context;
    }
}
