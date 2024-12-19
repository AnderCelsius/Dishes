namespace Dishes.Common.Models;

public class DishDTO : IDTO
{
    public Guid Id { get; set; }

    public required string Name { get; set; }
}