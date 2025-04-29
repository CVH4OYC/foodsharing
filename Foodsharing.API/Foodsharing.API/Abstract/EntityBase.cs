using System.ComponentModel.DataAnnotations;

namespace Foodsharing.API.Abstract;

public abstract class EntityBase
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
}
