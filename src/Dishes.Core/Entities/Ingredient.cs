using System.Diagnostics.CodeAnalysis;

namespace Dishes.Core.Entities;

public class Ingredient
{
  public Ingredient()
  { }

  [SetsRequiredMembers]
  public Ingredient(Guid id, string name)
  {
    Id = id;
    Name = name;
  }

  public Guid Id { get; set; }

  public required string Name { get; set; }

  public ICollection<Dish> Dishes { get; set; } = new List<Dish>();

  public ICollection<DishIngredient> DishIngredients { get; set; } = new List<DishIngredient>();
}
