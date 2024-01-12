using System.Diagnostics.CodeAnalysis;

namespace Dishes.Core.Entities;

public class Dish
{
    public Dish()
    {
    }

    [SetsRequiredMembers]
    public Dish(Guid id, string name)
    {
        Id = id;
        Name = name;
    }

    public Guid Id { get; set; }

    public required string Name { get; set; }

    public ICollection<Ingredient> Ingredients { get; set; } = new List<Ingredient>();

    public ICollection<DishIngredient> DishIngredients { get; set; } = new List<DishIngredient>();
}