namespace Dishes.Common.Models;

public class IngredientDTO : IDTO
{
    public Guid Id { get; set; }

    public required string Name { get; set; }
}