namespace Dishes.Core.Entities;

public class DishIngredient
{
    public Guid DishId { get; set; }
    public Dish Dish { get; set; } = null!;

    public Guid IngredientId { get; set; }
    public Ingredient Ingredient { get; set; } = null!;
}
